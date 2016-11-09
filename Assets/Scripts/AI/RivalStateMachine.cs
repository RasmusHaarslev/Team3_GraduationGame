using UnityEngine;
using System.Collections;

public class RivalStateMachine : CoroutineMachine
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

		if (character.isInCombat)
		{
			distanceToTarget = Vector3.Distance(transform.position, character.target.transform.position);
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
			yield return new TransitionTo(RoamState, DefaultTransition);
		}

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator RoamState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator FleeState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}


	IEnumerator EngageState()
	{
		agent.stoppingDistance = character.range;
		agent.SetDestination(character.target.transform.position);

		Debug.Log(gameObject.name + "is engaging " + character.target.name);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{
		agent.Stop();
		Debug.Log("Yasmin Attacking");
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DefaultTransition(StateRoutine from, StateRoutine to)
	{
		yield return new WaitForSeconds(transitionTime);
	}
}
