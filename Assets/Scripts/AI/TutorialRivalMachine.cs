using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialRivalMachine : CoroutineMachine
{
	public float transitionTime = 0.05f;

	NavMeshAgent agent;
	TutorialCharacter character;
	Vector3 originalPosition;
	public float distanceToTarget = float.MaxValue;
	public float fleeHealthLimit = 0.5f;
	public float averageHealth;
	private PointOfInterestManager poimanager;

	void OnEnable()
	{
		character = GetComponent<TutorialCharacter>();
		agent = GetComponent<NavMeshAgent>();
		originalPosition = transform.position;
		EventManager.Instance.StartListening<FleeStateEvent>(OpponentsFleeing);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<FleeStateEvent>(OpponentsFleeing);
	}

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	protected override StateRoutine InitialState
	{
		get
		{
			poimanager = transform.parent.parent.GetComponent<PointOfInterestManager>();
			return StartState;
		}
	}

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

	IEnumerator StartState()
	{
		if (character.isDead)
		{
			yield return new TransitionTo(DeadState, DefaultTransition);
		}
		averageHealth = poimanager.GetAverageCharactersHealth();
		if (averageHealth < fleeHealthLimit * poimanager.originalAverageHealth)
		{
			if (!character.isFleeing)
			{
				EventManager.Instance.TriggerEvent(new EnemyDeathEvent(gameObject));
				character.isFleeing = true;
			}
			yield return new TransitionTo(FleeState, DefaultTransition);
		}
		if (character.isInCombat)
		{
			if (character.currentOpponents.Count != 0)
			{
				if (character.target != null)
				{
					if (character.target.GetComponent<TutorialHunterCharacter>() != null)
					{
						if (!character.target.GetComponent<TutorialHunterCharacter>().isDead)
						{
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
					else if (character.target.GetComponent<TutorialPlayerCharacter>() != null)
					{
						if (!character.target.GetComponent<TutorialPlayerCharacter>().isDead)
						{
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
				} else
				{
					character.target = character.FindNearestEnemy();
				}
			}
			else
			{
				character.isInCombat = false;
			}

		}
		else
		{
			yield return new TransitionTo(RoamState, DefaultTransition);
		}

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator RoamState()
	{
		character.animator.SetBool("isAware", false);
		agent.Resume();
		agent.stoppingDistance = 1.2f;
		agent.SetDestination(originalPosition + new Vector3(0, 0, 0.5f));
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DeadState()
	{
		yield return new TransitionTo(DeadState, DefaultTransition);
	}

	IEnumerator FleeState()
	{
		if (!character.isDead)
		{
			character.animator.SetBool("isAware", false);
			character.isInCombat = false;
			agent.Resume();
			agent.stoppingDistance = 1.2f;
			agent.SetDestination(GameObject.FindGameObjectWithTag("FleePoint").transform.position);
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator EngageState()
	{
		character.animator.SetBool("isAware", false);
		if (!character.isDead && character.target != null)
		{
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
		if (!character.isDead)
		{
			agent.Stop();
			if (character.target != null)
			{
				character.RotateTowards(character.target.transform);
				character.animator.SetTrigger("Attack");
				yield return new WaitForSeconds(character.damageSpeed);
				character.DealDamage();
			}
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DefaultTransition(StateRoutine from, StateRoutine to)
	{
		yield return new WaitForSeconds(transitionTime);
	}

	private void OpponentsFleeing(FleeStateEvent e)
	{
		character.currentOpponents.Clear();
		character.isInCombat = false;
	}
}
