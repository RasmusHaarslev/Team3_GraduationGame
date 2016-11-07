using UnityEngine;
using System.Collections;

public class WolfStateMachine : CoroutineMachine {

	public float transitionTime = 0.05f;

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

	IEnumerator FindTargetState()
	{

		yield return new TransitionTo(EngageState, DefaultTransition);
	}

	IEnumerator EngageState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DefaultTransition(StateRoutine from, StateRoutine to)
	{
		yield return new WaitForSeconds(transitionTime);
	}
}
