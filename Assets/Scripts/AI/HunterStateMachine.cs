using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class HunterStateMachine : CoroutineMachine
{
	#region On Enable and Disable

	void OnEnable()
	{
		agent = GetComponent<NavMeshAgent>();
		leader = GameObject.FindGameObjectWithTag("Player");
		traits = GetComponent<Hunters>();
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
		inCombatCommand = InCombatCommand.Defense;
	}

	private void Offense(OffensiveStateEvent e)
	{
		inCombatCommand = InCombatCommand.Offense;
	}

	#endregion

	public float transitionTime = 0.05f;

	public bool inCombat = false;

	Hunters traits;

	// TODO: Should be in character script:
	public enum OutOfCombatCommand { Stay, Follow };
	public enum InCombatCommand { Offense, Defense, Fleeing };
	public OutOfCombatCommand outOfCombatCommand = OutOfCombatCommand.Follow; // instead get from character script
	public InCombatCommand inCombatCommand = InCombatCommand.Offense; // instead get from character script

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
		if (inCombat)
		{
			if (inCombatCommand == InCombatCommand.Offense)
			{
				// TODO
			}
			else if (inCombatCommand == InCombatCommand.Defense)
			{
				// TODO
			}
			else if (inCombatCommand == InCombatCommand.Fleeing)
			{
				if (fleePosition == null)
				{
					Debug.Log("No flee position assigned");
				}
				else
				{
					agent.SetDestination(fleePosition);
				}
				// TODO
			}
		}
		else
		{
			if (outOfCombatCommand == OutOfCombatCommand.Follow)
			{
				// TODO
				yield return new TransitionTo(FollowState, DefaultTransition);
			}
			else
			{
				// TODO
				yield return new TransitionTo(StayState, DefaultTransition);
			}
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

	//private fearfulCheck()
	//{
	//	 TODO:
	//	 if (character.health < character.maxHealth*fearfulFleePercentage) 
	//	 {
	//			return true;
	//	 } else {
	//			return false;
	//	}
		
	//}


	
}

