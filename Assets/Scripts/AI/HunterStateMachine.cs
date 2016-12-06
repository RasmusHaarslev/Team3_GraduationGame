using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class HunterStateMachine : CoroutineMachine
{
	#region On Enable and Disable

	void OnEnable()
	{
		character = GetComponent<Character>();
		agent = GetComponent<NavMeshAgent>();
		leader = GameObject.FindGameObjectWithTag("Player");
		formation = leader.GetComponent<Formation>();
		EventManager.Instance.StartListening<OffensiveStateEvent>(Offense);
		EventManager.Instance.StartListening<DefendStateEvent>(Defense);
		EventManager.Instance.StartListening<FollowStateEvent>(Follow);
		EventManager.Instance.StartListening<StayStateEvent>(Stay);
		EventManager.Instance.StartListening<FleeStateEvent>(Flee);
		EventManager.Instance.StartListening<StopFleeEvent>(StopFlee);
		if (GameObject.FindGameObjectWithTag("FleePoint") != null)
		{
			fleePosition = GameObject.FindGameObjectWithTag("FleePoint").transform.position;
		}
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<OffensiveStateEvent>(Offense);
		EventManager.Instance.StopListening<DefendStateEvent>(Defense);
		EventManager.Instance.StopListening<FollowStateEvent>(Follow);
		EventManager.Instance.StopListening<StayStateEvent>(Stay);
		EventManager.Instance.StopListening<FleeStateEvent>(Flee);
		EventManager.Instance.StopListening<StopFleeEvent>(StopFlee);
	}

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	#endregion

	#region Functions for events

	private void Defense(DefendStateEvent e)
	{
		ProjectCommand();
		combatCommandState = CombatCommandState.Defense;
	}

	private void Offense(OffensiveStateEvent e)
	{
		ProjectCommand();
		combatCommandState = CombatCommandState.Offense;
	}
	private void Follow(FollowStateEvent e)
	{
		ProjectCommand();
		outOfCombatCommandState = OutOfCombatCommandState.Follow;
	}

	private void StopFlee(StopFleeEvent e)
	{
		if (!character.isDead)
		{
			character.animator.SetBool("isFleeing", false);
			agent.Stop();
			character.isFleeing = false;
			stopFleeing = true;
			combatCommandState = CombatCommandState.Offense;
		}
	}

	private void Stay(StayStateEvent e)
	{
		ProjectCommand();
		outOfCombatCommandState = OutOfCombatCommandState.Stay;
	}
	private void Flee(FleeStateEvent e)
	{
		ProjectCommand();
		if (combatTrait != CharacterValues.CombatTrait.BraveFool)
		{
			character.animator.SetBool("isFleeing", true);
			combatCommandState = CombatCommandState.Flee;
		}
		else
		{
			ProjectTrait(combatTrait, CharacterValues.TargetTrait.NoTrait);
		}


	}

	#endregion

	public float transitionTime = 0.1f;
	float fearfulHealthLimit = 0.25f;
	public int maxLowAttentionSpanCounter = 1;
	int lowAttentionSpanCounter = 3;
	public float fleeSpeed = 4f;
	public float stayAttackDistance = 10f;
	bool stopFleeing = false;

	// Trait visualisation
	public GameObject traitProjection;
	bool traitVisualised = false;
	public GameObject commandVisualisation;

	public bool attacked = false;
	Character character;
	NavMeshAgent agent;
	GameObject leader;
	Formation formation;
	bool isTraitProjected;

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
	public CharacterValues.TargetTrait targetTrait = CharacterValues.TargetTrait.NoTrait;
	public CharacterValues.CombatTrait combatTrait = CharacterValues.CombatTrait.NoTrait;
	public OutOfCombatCommandState outOfCombatCommandState = OutOfCombatCommandState.Follow;
	WaitForSeconds transition;
	public Vector3 fleePosition;
	public float distanceToTarget = float.MaxValue;
	public int partyID = 0;

	void Update()
	{
		if (character.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !character.isDead)
		{
			agent.Stop();
		}
	}

	protected override StateRoutine InitialState
	{
		get
		{
			combatTrait = character.characterBaseValues.combatTrait;
			targetTrait = character.characterBaseValues.targetTrait;
			transition = new WaitForSeconds(transitionTime);
			return StartState;
		}
	}

	IEnumerator DefaultTransition(StateRoutine from, StateRoutine to)
	{
		yield return transition;
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
				switch (targetTrait)
				{
					case CharacterValues.TargetTrait.Codependant:
						character.target = CodependantTarget();

						if (!isTraitProjected)
							ProjectTrait(CharacterValues.CombatTrait.NoTrait, targetTrait);
						if (!leader.GetComponent<MoveScript>().attacking)
						{
							yield return new TransitionTo(FollowState, DefaultTransition);
						}
						break;
					case CharacterValues.TargetTrait.Loyal:
						GameObject loyalTarget = LoyalTarget();
						if (!isTraitProjected)
							ProjectTrait(CharacterValues.CombatTrait.NoTrait, targetTrait);
						if (loyalTarget != null)
						{
							character.target = loyalTarget;
						}
						break;
				}
				if (outOfCombatCommandState == OutOfCombatCommandState.Stay && distanceToTarget > stayAttackDistance && combatTrait != CharacterValues.CombatTrait.Clingy)
				{
					yield return new TransitionTo(StayState, DefaultTransition);
				}

				if (combatCommandState == CombatCommandState.Flee || (combatTrait == CharacterValues.CombatTrait.Fearful && character.currentHealth < (character.health * fearfulHealthLimit)))
				{
					if (combatTrait == CharacterValues.CombatTrait.Fearful)
					{
						if (!isTraitProjected)
							ProjectTrait(combatTrait, CharacterValues.TargetTrait.NoTrait);
					}
					yield return new TransitionTo(FleeState, DefaultTransition);
				}
				else
				{
					if (character.currentOpponents.Count != 0)
					{
						if (combatCommandState == CombatCommandState.Offense)
						{
							if (character.target != null && character.target.GetComponent<Character>() != null)
							{
								if (!character.target.GetComponent<Character>().isDead)
								{
									if (lowAttentionSpanCounter <= 0 && targetTrait == CharacterValues.TargetTrait.LowAttentionSpan)
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
												if (!isTraitProjected)
													ProjectTrait(CharacterValues.CombatTrait.NoTrait, targetTrait);
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
									character.target = character.FindNearestEnemy();
								}
							}
						}
						else if (combatCommandState == CombatCommandState.Defense)
						{
							if (character.target.GetComponent<Character>() != null)
							{
								if (character.target.GetComponent<Character>().isDead)
								{
									character.currentOpponents.Remove(character.target);
									character.target = character.FindNearestEnemy();
								}
								yield return new TransitionTo(DefenseState, DefaultTransition);
							}
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
				agent.updateRotation = true;
				if (combatCommandState == CombatCommandState.Flee)
				{
					if (combatTrait != CharacterValues.CombatTrait.BraveFool)
					{
						yield return new TransitionTo(FleeState, DefaultTransition);
					}
				}
				if (outOfCombatCommandState == OutOfCombatCommandState.Stay && combatTrait != CharacterValues.CombatTrait.Clingy)
				{
					yield return new TransitionTo(StayState, DefaultTransition);
				}
				else
				{
					agent.Resume();
					if (combatTrait == CharacterValues.CombatTrait.Clingy && outOfCombatCommandState == OutOfCombatCommandState.Stay)
					{
						if (!isTraitProjected)
							ProjectTrait(combatTrait, CharacterValues.TargetTrait.NoTrait);
					}
					yield return new TransitionTo(FollowState, DefaultTransition);
				}
			}
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator FollowState()
	{
		character.animator.SetBool("isAware", false);
		character.isInCombat = false;
		if (!character.isDead)
		{
			agent.Resume();
			agent.stoppingDistance = 1.2f;
			agent.SetDestination(formation.formationPositions[gameObject].position);
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
		character.animator.SetBool("isAware", false);
		agent.SetDestination(fleePosition);
		agent.speed = fleeSpeed;
		character.isFleeing = true;
		character.target = null;
		character.isInCombat = false;
		agent.Resume();
		agent.stoppingDistance = 1.2f;
		yield return new WaitForSeconds(1);
		if (agent.remainingDistance < agent.stoppingDistance)
		{
			if (new Vector3(agent.destination.x, 0, agent.destination.z) == new Vector3(fleePosition.x, 0, fleePosition.z))
			{
				gameObject.SetActive(false);
			}
		}
		if (stopFleeing)
		{
			agent.speed = 2.8f;
			character.isFleeing = false;
			yield return new TransitionTo(StartState, DefaultTransition);
		}
		yield return new TransitionTo(FleeState, DefaultTransition);
	}

	IEnumerator EngageState()
	{
		agent.updateRotation = true;
		character.animator.SetBool("isAware", false);
		if (character.target != null && character.target.GetComponent<Character>() != null)
		{
			if (!character.target.GetComponent<Character>().isInCombat)
			{
				character.isInCombat = false;
				yield return new TransitionTo(StartState, DefaultTransition);
			}
			agent.Resume();
			agent.stoppingDistance = character.range;
			agent.SetDestination(character.target.transform.position);
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{
		if (!character.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
			character.animator.SetBool("isAware", true);
			transform.position = transform.position;
			agent.Stop();
		}
		else
		{
			character.animator.SetBool("isAware", false);
		}
		if (character.target != null)
		{
			if (character.target.GetComponent<Character>() != null)
			{
				if (!character.target.GetComponent<Character>().isInCombat)
				{
					character.isInCombat = false;
					yield return new TransitionTo(StartState, DefaultTransition);
				}
			}
			agent.updateRotation = false;
			character.RotateTowards(character.target.transform);
			character.animator.SetTrigger("Attack");
			yield return new WaitForSeconds(0.60f);
			character.DealDamage();
			lowAttentionSpanCounter--;
			yield return new WaitForSeconds(character.damageSpeed);
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DeadState()
	{
		yield return new TransitionTo(DeadState, DefaultTransition);
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

	private GameObject CodependantTarget()
	{
		GameObject target;
		target = leader.GetComponent<Character>().target;
		return target;
	}

	private GameObject LoyalTarget()
	{
		GameObject target = null;
		foreach (GameObject enemy in character.currentOpponents)
		{
			if (enemy.GetComponent<Character>() != null)
			{
				if (enemy.GetComponent<Character>().target == leader)
				{
					target = enemy;
					break;
				}
			}
		}
		if (target == null)
		{
			target = character.FindRandomEnemy();
		}
		return target;
	}

	private void ProjectTrait(CharacterValues.CombatTrait combatTrait = CharacterValues.CombatTrait.NoTrait, CharacterValues.TargetTrait targetTrait = CharacterValues.TargetTrait.NoTrait)
	{
		isTraitProjected = true;
		StartCoroutine(TraitProjector(combatTrait, targetTrait));
	}

	IEnumerator TraitProjector(CharacterValues.CombatTrait combatTrait = CharacterValues.CombatTrait.NoTrait, CharacterValues.TargetTrait targetTrait = CharacterValues.TargetTrait.NoTrait)
	{
		GameObject proj = Instantiate(traitProjection);
		if (combatTrait != CharacterValues.CombatTrait.NoTrait)
		{
			proj.GetComponent<traitText>().trait = combatTrait.ToString();
		}
		else
		{
			proj.GetComponent<traitText>().trait = targetTrait.ToString();
		}
		proj.transform.SetParent(gameObject.transform, false);
		proj.transform.eulerAngles = new Vector3(90, 0, 0);
		yield return new WaitForSeconds(2f);
		Destroy(proj);
		isTraitProjected = false;
		yield return null;
	}

	public void ProjectCommand()
	{
		Instantiate(commandVisualisation, transform.position + new Vector3(0, 2.2f, 0.2f), transform.rotation, gameObject.transform);
	}
}

