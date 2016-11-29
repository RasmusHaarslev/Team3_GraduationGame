using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RivalStateMachine : CoroutineMachine
{
	public float transitionTime = 0.05f;

	NavMeshAgent agent;
	Character character;
	Vector3 originalPosition;
	public float distanceToTarget = float.MaxValue;
	public float fleeHealthLimit = 0.5f;
	public float averageHealth;
	private PointOfInterestManager poimanager;

	void OnEnable()
	{
		character = GetComponent<Character>();
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
				//character.RotateTowards(character.target.transform);
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
			EventManager.Instance.TriggerEvent(new EnemyDeathEvent(gameObject));
			character.isFleeing = true;
			yield return new TransitionTo(FleeState, DefaultTransition);
		}
		if (character.isInCombat)
		{
			if (character.currentOpponents.Count != 0)
			{
				if (!character.target.GetComponent<Character>().isDead)
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
			else
			{
				character.isInCombat = false;
			}
		}
		else
		{
			agent.updatePosition = true;
			agent.updateRotation = true;
			yield return new TransitionTo(RoamState, DefaultTransition);
		}
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator RoamState()
	{
		agent.Resume();
		character.animator.SetBool("isAware", false);
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
		agent.updateRotation = true;
		if (!character.isDead)
		{
			character.animator.SetBool("isAware", false);
			agent.Resume();
			agent.stoppingDistance = 1.2f;
			character.isInCombat = false;
			agent.SetDestination(GameObject.FindGameObjectWithTag("FleePoint").transform.position);
		}
		yield return new TransitionTo(FleeState, DefaultTransition);
	}

	IEnumerator EngageState()
	{
		agent.updateRotation = true;
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
			agent.updateRotation = false;
			character.RotateTowards(character.target.transform);
			character.animator.SetTrigger("Attack");
			yield return new WaitForSeconds(0.60f);
			character.DealDamage();
			yield return new WaitForSeconds(character.damageSpeed);

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
