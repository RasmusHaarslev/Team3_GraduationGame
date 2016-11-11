using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class HunterStateMachine : CoroutineMachine
{
	public enum TargetTrait
	{
		NoTrait,
		Foolhardy,
		StubbornDefender,
		PacifistSoul,
		Bully,
		Codependant,
		Helpful,
		GlorySeeker,
		VeryUnlikable,
		LowAttentionSpan,
		Loyal,

	}
	public enum CombatTrait
	{
		NoTrait,
		BraveFool,
		Fearful,
		Excitable,
		Clingy,
		Desperate,
		Vengeful
	}

	#region On Enable and Disable

	void OnEnable()
	{
		character = GetComponent<Character>();
		agent = GetComponent<NavMeshAgent>();
		leader = GameObject.FindGameObjectWithTag("Player");
		EventManager.Instance.StartListening<OffensiveStateEvent>(Offense);
		EventManager.Instance.StartListening<DefendStateEvent>(Defense);
		EventManager.Instance.StartListening<FollowStateEvent>(Follow);
		EventManager.Instance.StartListening<StayStateEvent>(Stay);
		EventManager.Instance.StartListening<FleeStateEvent>(Flee);
		EventManager.Instance.StartListening<AllyDeathEvent>(Death);
	}


	void OnDisable()
	{
		EventManager.Instance.StopListening<OffensiveStateEvent>(Offense);
		EventManager.Instance.StopListening<DefendStateEvent>(Defense);
		EventManager.Instance.StopListening<FollowStateEvent>(Follow);
		EventManager.Instance.StopListening<StayStateEvent>(Stay);
		EventManager.Instance.StopListening<FleeStateEvent>(Flee);
		EventManager.Instance.StopListening<AllyDeathEvent>(Death);
	}


	#endregion

	#region Functions for events

	private void Death(AllyDeathEvent e)
	{
		if (combatTrait == CombatTrait.Vengeful)
		{
			character.damage += damageIncrease;
		}
	}

	private void Defense(DefendStateEvent e)
	{
		combatCommandState = CombatCommandState.Defense;
	}

	private void Offense(OffensiveStateEvent e)
	{
		combatCommandState = CombatCommandState.Offense;
	}
	private void Follow(FollowStateEvent e)
	{
		outOfCombatCommandState = OutOfCombatCommandState.Follow;
	}
	private void Stay(StayStateEvent e)
	{
		outOfCombatCommandState = OutOfCombatCommandState.Stay;
	}
	private void Flee(FleeStateEvent e)
	{
		combatCommandState = CombatCommandState.Flee;
	}

	#endregion

	public float transitionTime = 0.05f;
	public float fearfulHealthLimit = 25;
	public int maxLowAttentionSpanCounter = 3;
	int lowAttentionSpanCounter = 3;
	public int desperateHealthLimit = 25;
	public int damageIncrease = 10;
	bool damageIncreased = false;

	public bool attacked = false;
	Character character;
	NavMeshAgent agent;
	GameObject leader;

	public enum CombatCommandState
	{
		Offense,
		Defense,
		Flee
	}

	public enum OutOfCombatCommandState
	{
		Follow,
		Stay
	}

	[SerializeField]
	public CombatCommandState combatCommandState = CombatCommandState.Offense;
	public TargetTrait targetTrait = TargetTrait.NoTrait;
	public CombatTrait combatTrait = CombatTrait.NoTrait;
	public OutOfCombatCommandState outOfCombatCommandState = OutOfCombatCommandState.Follow;

	public Vector3 fleePosition;
	public float distanceToTarget = float.MaxValue;
	public int partyID = 0;

	void Update()
	{
		if (character.target != null)
		{
			character.RotateTowards(character.target.transform);
		}

		if (combatTrait == CombatTrait.Desperate && character.currentHealth <= desperateHealthLimit)
		{
			if (!damageIncreased)
			{
				character.damage += damageIncrease;
				damageIncreased = true;
			}
		}
		else if (combatTrait == CombatTrait.Desperate && character.currentHealth > desperateHealthLimit)
		{
			damageIncreased = false;
		}
	}

	protected override StateRoutine InitialState
	{
		get
		{
			return StartState;
		}
	}

	IEnumerator DefaultTransition(StateRoutine from, StateRoutine to)
	{
		yield return new WaitForSeconds(transitionTime);
	}

	//This state will make all checks and transition according to them
	IEnumerator StartState()
	{
		if (character.isDead)
		{
			yield return new TransitionTo(DeadState, DefaultTransition);
		}
		else
		{
			if (character.isInCombat)
			{
				if (targetTrait == TargetTrait.Bully)
				{
					character.target = BullyTarget();
				}
				else if (targetTrait == TargetTrait.GlorySeeker)
				{
					character.target = GlorySeekerTarget();
				}
				if (combatCommandState == CombatCommandState.Flee && combatTrait != CombatTrait.BraveFool || (combatTrait == CombatTrait.Fearful && character.currentHealth < fearfulHealthLimit))
				{
					yield return new TransitionTo(FleeState, DefaultTransition);
				}
				else
				{
					if (character.currentOpponents.Count != 0)
					{
						if ((combatCommandState == CombatCommandState.Offense || targetTrait == TargetTrait.Foolhardy || (combatCommandState == CombatCommandState.Flee && combatTrait == CombatTrait.BraveFool)) && targetTrait != TargetTrait.StubbornDefender)
						{
							if (!character.target.GetComponent<Character>().isDead)
							{
								if (lowAttentionSpanCounter <= 0 && targetTrait == TargetTrait.LowAttentionSpan)
								{
									GameObject formerTarget = character.target;
									lowAttentionSpanCounter = maxLowAttentionSpanCounter;
									while (formerTarget == character.target)
									{
										if (character.currentOpponents.Count <= 1)
										{
											break;
										}
										else
										{
											character.target = character.FindRandomEnemy();
										}
									}
								}
								distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(character.target.transform.position.x, 0, character.target.transform.position.z));
								if (distanceToTarget < agent.stoppingDistance)
								{
									yield return new TransitionTo(CombatState, DefaultTransition);
								}
								else
								{
									yield return new TransitionTo(EngageState, DefaultTransition);
								}
							}
							else
							{
								character.currentOpponents.Remove(character.target);
								if (targetTrait == TargetTrait.Bully)
								{
									character.target = BullyTarget();
								}
								else if (targetTrait == TargetTrait.GlorySeeker)
								{
									character.target = GlorySeekerTarget();
								}
								else
								{
									character.target = character.FindNearestEnemy();
								}
							}
						}
						else if (combatCommandState == CombatCommandState.Defense || targetTrait == TargetTrait.StubbornDefender)
						{
							if (character.target.GetComponent<Character>().isDead)
							{
								character.currentOpponents.Remove(character.target);
								if (targetTrait == TargetTrait.Bully)
								{
									character.target = BullyTarget();
								}
								else if (targetTrait == TargetTrait.GlorySeeker)
								{
									character.target = GlorySeekerTarget();
								}
								else
								{
									character.target = character.FindNearestEnemy();
								}
							}
							yield return new TransitionTo(DefenseState, DefaultTransition);
						}
					}
					else
					{
						character.isInCombat = false;
					}
				}
			}
			else
			{
				if (outOfCombatCommandState == OutOfCombatCommandState.Stay && combatTrait != CombatTrait.Clingy)
				{
					yield return new TransitionTo(StayState, DefaultTransition);
				}
				else
				{
					agent.Resume();
					yield return new TransitionTo(FollowState, DefaultTransition);
				}
			}
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator FollowState()
	{
		agent.Resume();
		agent.stoppingDistance = 0;
		// TODO make these dynamic:
		if (partyID == 1)
		{
			agent.SetDestination(leader.transform.position - leader.transform.forward * 2 - leader.transform.right * 2);
		}
		else if (partyID == 2)
		{
			agent.SetDestination(leader.transform.position - leader.transform.forward * 4);
		}
		else if (partyID == 3)
		{
			agent.SetDestination(leader.transform.position - leader.transform.forward * 2 + leader.transform.right * 2);
		}
		else if (partyID == 4)
		{
			agent.SetDestination(leader.transform.position - leader.transform.forward * 4 + leader.transform.right * 2);
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator StayState()
	{
		agent.Stop();
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator FleeState()
	{
		character.target = null;
		agent.Resume();
		agent.stoppingDistance = 0;
		agent.SetDestination(GameObject.FindGameObjectWithTag("FleePoint").transform.position);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator EngageState()
	{
		agent.Resume();
		agent.stoppingDistance = character.range;
		agent.SetDestination(character.target.transform.position);

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{
		agent.Stop();
		character.RotateTowards(character.target.transform);
		yield return new WaitForSeconds(character.damageSpeed);
		character.DealDamage();
		lowAttentionSpanCounter--;
		character.RotateTowards(character.target.transform);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DeadState()
	{
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DefenseState()
	{

		if (character.isInCombat)
		{
			agent.Stop();
		}
		else
		{
			agent.Resume();
		}

		if (attacked == true)
		{
			yield return new TransitionTo(CombatState, DefaultTransition);
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	private GameObject BullyTarget()
	{
		int min = int.MaxValue;
		GameObject target = null;
		foreach (GameObject opponent in character.currentOpponents)
		{
			if (opponent.GetComponent<Character>().characterBaseValues.tier < min)
			{
				target = opponent;
				min = opponent.GetComponent<Character>().characterBaseValues.tier;
			}
		}
		return target;
	}

	private GameObject GlorySeekerTarget()
	{
		int max = int.MinValue;
		GameObject target = null;
		foreach (GameObject opponent in character.currentOpponents)
		{
			if (opponent.GetComponent<Character>().characterBaseValues.tier > max)
			{
				target = opponent;
				max = opponent.GetComponent<Character>().characterBaseValues.tier;
			}
		}
		return target;
	}
}

