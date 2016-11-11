﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class HunterStateMachine : CoroutineMachine
{
	public enum TargetTrait
	{
		NoTrait,
		Foolhardy,
		StubbornDefender,
		PacifistSoul,
		Bully,
		Codependant,
		Helpful,
		GlorySeeker,
		VeryUnlikable,
		LowAttentionSpan,
		Loyal,

	}
	public enum CombatTrait
	{
		NoTrait,
		BraveFool,
		Fearful,
		Excitable,
		Clingy,
		Desperate,
		Vengeful
	}

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
		combatCommandState = CombatCommandState.Defense;
	}

	private void Offense(OffensiveStateEvent e)
	{
		combatCommandState = CombatCommandState.Offense;
	}

	#endregion

	public float transitionTime = 0.05f;

	public bool attacked = false;
	Character character;
	NavMeshAgent agent;
	GameObject leader;


	public enum CombatCommandState
	{
		Offense,
		Defense,
		Flee
	}
	[SerializeField]
	public CombatCommandState combatCommandState = CombatCommandState.Offense;
	public TargetTrait targetTrait = TargetTrait.NoTrait;
	public CombatTrait combatTrait = CombatTrait.NoTrait;

	public Vector3 fleePosition;
	public float distanceToTarget = float.MaxValue;
	public int partyID = 0;

	void Update()
	{
		if (character.target != null)
		{
			character.RotateTowards(character.target.transform);
		}
	

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
				if ((combatCommandState == CombatCommandState.Offense || targetTrait == TargetTrait.Foolhardy) && targetTrait != TargetTrait.StubbornDefender)
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
				else if (combatCommandState == CombatCommandState.Defense || targetTrait == TargetTrait.StubbornDefender)
				{
					if (character.target.GetComponent<Character>().isDead)
					{
						character.currentOpponents.Remove(character.target);
						character.target = character.FindNearestEnemy();
					}
					yield return new TransitionTo(DefenseState, DefaultTransition);
				}
			}
			else
			{
				character.isInCombat = false;
			}

		}
		else
		{
			agent.Resume();
			yield return new TransitionTo(FollowState, DefaultTransition);
		}

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator FollowState()
	{
		agent.Resume();
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
		agent.stoppingDistance = character.range;
		agent.SetDestination(character.target.transform.position);

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState()
	{
		agent.Stop();
		character.RotateTowards(character.target.transform);
		yield return new WaitForSeconds(character.damageSpeed);
		character.DealDamage();
		character.RotateTowards(character.target.transform);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DeadState()
	{
		Debug.Log(gameObject.name + " dead");
		yield return new TransitionTo(StartState, DefaultTransition);
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



}

