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
		EventManager.Instance.StartListening<OffensiveStateEvent>(Offense);
		EventManager.Instance.StartListening<DefendStateEvent>(Defense);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<OffensiveStateEvent>(Offense);
		EventManager.Instance.StopListening<DefendStateEvent>(Defense);
	}

	#endregion

	#region Functions for events

	private void Defense(DefendStateEvent e)
	{

	}

	private void Offense(OffensiveStateEvent e)
	{

	}

	#endregion

	public float transitionTime = 0.05f;

	Character character;

	public Vector3 fleePosition;

	NavMeshAgent agent;
	GameObject leader;

	public GameObject formationPosition;
	public int partyID = 0;

	void Update()
	{

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
		if (character.isInCombat)
		{
			yield return new TransitionTo(EngageState, DefaultTransition);
		}
		else
		{
			yield return new TransitionTo(FollowState, DefaultTransition);
		}

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator FollowState()
	{
		agent.stoppingDistance = 0;
		agent.SetDestination(formationPosition.transform.position);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator StayState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}


	IEnumerator FleeState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator EngageState()
	{
		Debug.Log("Attacking");
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}

}

