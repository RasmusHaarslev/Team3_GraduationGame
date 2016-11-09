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
	NavMeshAgent agent;
	GameObject leader;

	public Vector3 fleePosition;
	public float distanceToTarget = float.MaxValue;
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
			yield return new TransitionTo(FollowState, DefaultTransition);
		}

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator FollowState()
	{
		agent.Resume();
		Debug.Log(gameObject.name + " is following");
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

		yield return new TransitionTo(StartState, DefaultTransition);
	}


	IEnumerator FleeState()
	{

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator EngageState()
	{
		agent.Resume();
		Debug.Log("hunter engaging");
		agent.stoppingDistance = character.range;
		agent.SetDestination(character.target.transform.position);

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{
		agent.Stop();
		yield return new WaitForSeconds(1/character.damageSpeed);
		Debug.Log("hunter dealing damage");
		character.DealDamage();
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	 IEnumerator DeadState()
	{
		Debug.Log("dead");
		yield return new TransitionTo(StartState, DefaultTransition);
	}

}

