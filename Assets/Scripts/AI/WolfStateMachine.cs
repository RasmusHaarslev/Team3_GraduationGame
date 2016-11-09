using UnityEngine;
using System.Collections;

public class WolfStateMachine : CoroutineMachine
{

	public float transitionTime = 0.05f;

	NavMeshAgent agent;
	Character character;


	public float distanceToTarget = float.MaxValue;

	void OnEnable()
	{
		character = GetComponent<Character>();
		agent = GetComponent<NavMeshAgent>();

	}

	void OnDisable()
	{

	}


	protected override StateRoutine InitialState
	{
		get
		{
			return StartState;
		}
	}

	void Update()
	{

	}

	IEnumerator StartState()
	{
		if (character.isDead)
		{
			yield return new TransitionTo(DeadState, DefaultTransition);
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
			yield return new TransitionTo(RoamState, DefaultTransition);
		}

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator RoamState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DeadState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}


	IEnumerator FleeState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}


	IEnumerator EngageState()
	{
		agent.Resume();
		Debug.Log("engages");
		agent.stoppingDistance = character.range;
		agent.SetDestination(character.target.transform.position);
		Debug.Log("target: " + character.target.name);

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{
		agent.Stop();
		yield return new WaitForSeconds(1/character.damageSpeed);
		Debug.Log("yasmin dealing damage");
		character.DealDamage();
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DefaultTransition(StateRoutine from, StateRoutine to)
	{
		yield return new WaitForSeconds(transitionTime);
	}
}
