using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class TutorialHunterStateMachine : CoroutineMachine
{
	#region On Enable and Disable

	void OnEnable()
	{
		character = GetComponent<TutorialHunterCharacter>();
		agent = GetComponent<NavMeshAgent>();
		leader = GameObject.FindGameObjectWithTag("Player");
		formation = leader.GetComponent<Formation>();
		EventManager.Instance.StartListening<OffensiveStateEvent>(Offense);
		EventManager.Instance.StartListening<DefendStateEvent>(Defense);
		EventManager.Instance.StartListening<FollowStateEvent>(Follow);
		EventManager.Instance.StartListening<StayStateEvent>(Stay);
		EventManager.Instance.StartListening<FleeStateEvent>(Flee);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<OffensiveStateEvent>(Offense);
		EventManager.Instance.StopListening<DefendStateEvent>(Defense);
		EventManager.Instance.StopListening<FollowStateEvent>(Follow);
		EventManager.Instance.StopListening<StayStateEvent>(Stay);
		EventManager.Instance.StopListening<FleeStateEvent>(Flee);
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
	private void Stay(StayStateEvent e)
	{
		ProjectCommand();
		outOfCombatCommandState = OutOfCombatCommandState.Stay;
	}
	private void Flee(FleeStateEvent e)
	{
		ProjectCommand();
		combatCommandState = CombatCommandState.Flee;
	}

	#endregion

	public float transitionTime = 0.05f;
	public float fearfulHealthLimit = 25;
	public int maxLowAttentionSpanCounter = 1;
	int lowAttentionSpanCounter = 3;

	// Trait visualisation
	public GameObject traitProjection;
	bool traitVisualised = false;
	public GameObject commandVisualisation;

	public bool attacked = false;
	TutorialHunterCharacter character;
	NavMeshAgent agent;
	GameObject leader;
	Formation formation;

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

	public Vector3 fleePosition;
	public float distanceToTarget = float.MaxValue;
	public int partyID = 0;

	void Update()
	{
		if (character.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !character.isDead)
		{
			agent.Stop();
		}
		if (character.target != null)
		{
			if (!character.isDead)
			{
				character.RotateTowards(character.target.transform);
			}
		}
	}

	protected override StateRoutine InitialState
	{
		get
		{
			//combatTrait = character.characterBaseValues.combatTrait;
			//targetTrait = character.characterBaseValues.targetTrait;
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
				switch (targetTrait)
				{
					case CharacterValues.TargetTrait.Codependant:
						character.target = CodependantTarget();
						ProjectTrait(CharacterValues.CombatTrait.NoTrait, targetTrait);
						if (!leader.GetComponent<MoveScript>().attacking)
						{
							character.isInCombat = false;
							yield return new TransitionTo(FollowState, DefaultTransition);
						}
						break;
					case CharacterValues.TargetTrait.Loyal:
						GameObject loyalTarget = LoyalTarget();
						ProjectTrait(CharacterValues.CombatTrait.NoTrait, targetTrait);
						if (loyalTarget != null)
						{
							character.target = loyalTarget;
						}
						break;
				}
				if (combatCommandState == CombatCommandState.Flee || (combatTrait == CharacterValues.CombatTrait.Fearful && character.currentHealth < fearfulHealthLimit))
				{
					if (combatTrait == CharacterValues.CombatTrait.Fearful)
					{
						ProjectTrait(combatTrait, CharacterValues.TargetTrait.NoTrait);
					}
					yield return new TransitionTo(FleeState, DefaultTransition);
				}
				else
				{
					if (character.currentOpponents.Count != 0)
					{
						if ((combatCommandState == CombatCommandState.Offense || (combatCommandState == CombatCommandState.Flee && combatTrait == CharacterValues.CombatTrait.BraveFool)))
						{
							if (character.target != null && character.target.GetComponent<TutorialCharacter>() != null)
							{
								if (!character.target.GetComponent<TutorialCharacter>().isDead)
								{
									if (combatTrait == CharacterValues.CombatTrait.BraveFool)
									{
										ProjectTrait(combatTrait, CharacterValues.TargetTrait.NoTrait);
									}
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
							else if (character.target != null && character.target.GetComponent<Character>() != null)
							{
								if (!character.target.GetComponent<Character>().isDead)
								{
									if (combatTrait == CharacterValues.CombatTrait.BraveFool)
									{
										ProjectTrait(combatTrait, CharacterValues.TargetTrait.NoTrait);
									}
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
							if (character.target.GetComponent<TutorialCharacter>() != null)
							{
								if (character.target.GetComponent<TutorialCharacter>().isDead)
								{
									character.currentOpponents.Remove(character.target);
									character.target = character.FindNearestEnemy();
								}
								yield return new TransitionTo(DefenseState, DefaultTransition);
							}
							else if (character.target.GetComponent<Character>() != null)
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
				if (combatTrait == CharacterValues.CombatTrait.Fearful && character.currentHealth < fearfulHealthLimit)
				{
					ProjectTrait(combatTrait, CharacterValues.TargetTrait.NoTrait);
					yield return new TransitionTo(FleeState, DefaultTransition);
				}
				if (combatCommandState == CombatCommandState.Flee)
				{
					yield return new TransitionTo(FleeState, DefaultTransition);
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
		character.target = null;
		character.isInCombat = false;
		agent.Resume();
		agent.stoppingDistance = 1.2f;
		agent.SetDestination(GameObject.FindGameObjectWithTag("FleePoint").transform.position);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator EngageState()
	{
		if (character.target.GetComponent<TutorialCharacter>() != null)
		{
			if (!character.target.GetComponent<TutorialCharacter>().isInCombat)
			{
				character.isInCombat = false;
				yield return new TransitionTo(StartState, DefaultTransition);
			}
		}
		else if (character.target.GetComponent<Character>() != null)
		{
			if (!character.target.GetComponent<Character>().isInCombat)
			{
				character.isInCombat = false;
				yield return new TransitionTo(StartState, DefaultTransition);
			}
		}
		agent.Resume();
		agent.stoppingDistance = character.range;
		agent.SetDestination(character.target.transform.position);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{
		agent.Stop();
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
			if (character.target.GetComponent<TutorialCharacter>() != null)
			{
				if (!character.target.GetComponent<TutorialCharacter>().isInCombat)
				{
					character.isInCombat = false;
					yield return new TransitionTo(StartState, DefaultTransition);
				}
			}
			character.RotateTowards(character.target.transform);
			character.animator.SetTrigger("Attack");
			yield return new WaitForSeconds(character.damageSpeed);
			character.DealDamage();
			lowAttentionSpanCounter--;
			yield return new TransitionTo(StartState, DefaultTransition);
		}
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
			if (enemy.GetComponent<TutorialCharacter>() != null)
			{
				if (enemy.GetComponent<TutorialCharacter>().target == leader)
				{
					target = enemy;
					break;
				}
			}
			if (enemy.GetComponent<Character>() != null)
			{
				if (enemy.GetComponent<Character>().target == leader)
				{
					target = enemy;
					break;
				}
			}
		}
		return target;
	}

	private void ProjectTrait(CharacterValues.CombatTrait combatTrait = CharacterValues.CombatTrait.NoTrait, CharacterValues.TargetTrait targetTrait = CharacterValues.TargetTrait.NoTrait)
	{
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
		yield return null;
	}

	public void ProjectCommand()
	{
		Instantiate(commandVisualisation, transform.position + new Vector3(0, 2.2f, 0.2f), transform.rotation, gameObject.transform);
	}
}

