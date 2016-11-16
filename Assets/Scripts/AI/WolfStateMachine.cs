using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WolfStateMachine : CoroutineMachine {
	public float transitionTime = 0.05f;

	NavMeshAgent agent;
	Character character;
	Vector3 originalPosition;
	public float distanceToTarget = float.MaxValue;
	public int fleeHealthLimit = 3;
	public float averageHealth;
	private PointOfInterestManager poimanager;

	void OnEnable() {
		character = GetComponent<Character>();
		agent = GetComponent<NavMeshAgent>();
		originalPosition = transform.position;
		EventManager.Instance.StartListening<FleeStateEvent>(OpponentsFleeing);
	}

	void OnDisable() {
		EventManager.Instance.StopListening<FleeStateEvent>(OpponentsFleeing);
	}

	protected override StateRoutine InitialState {
		get
		{
			poimanager = transform.parent.parent.GetComponent<PointOfInterestManager>();
			return StartState;
		}
	}

	void Update() {
		if (character.target != null) {
			character.RotateTowards(character.target.transform);
		}
	}

	IEnumerator StartState() {
		if (character.isDead) {
			yield return new TransitionTo(DeadState, DefaultTransition);
		}
		averageHealth = poimanager.GetAverageCharactersHealth();
		if (averageHealth < fleeHealthLimit) {
			yield return new TransitionTo(FleeState, DefaultTransition);
		}
		if (character.isInCombat) {
			if (character.currentOpponents.Count != 0) {
				if (!character.target.GetComponent<Character>().isDead) {
					distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(character.target.transform.position.x, 0, character.target.transform.position.z));
					if (distanceToTarget < agent.stoppingDistance) {
						yield return new TransitionTo(CombatState, DefaultTransition);
					}
					else {
						yield return new TransitionTo(EngageState, DefaultTransition);
					}
				}
				else {
					character.currentOpponents.Remove(character.target);
					character.target = character.FindNearestEnemy();
				}
			}
			else {
				character.isInCombat = false;
			}

		}
		else {
			yield return new TransitionTo(RoamState, DefaultTransition);
		}

		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator RoamState() {
		agent.Resume();
		agent.stoppingDistance = 0;
		agent.SetDestination(originalPosition);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DeadState() {
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator FleeState() {
		agent.Resume();
		agent.stoppingDistance = 0;
		agent.SetDestination(GameObject.FindGameObjectWithTag("FleePoint").transform.position);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator EngageState() {
		agent.Resume();
		agent.stoppingDistance = character.range;
		agent.SetDestination(character.target.transform.position);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator CombatState() {
		character.RotateTowards(character.target.transform);
		agent.Stop();
		yield return new WaitForSeconds(character.damageSpeed);
		character.DealDamage();
		character.RotateTowards(character.target.transform);
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator DefaultTransition(StateRoutine from, StateRoutine to) {
		yield return new WaitForSeconds(transitionTime);
	}

	private void OpponentsFleeing(FleeStateEvent e) {
		character.currentOpponents.Clear();
		character.isInCombat = false;
	}
}
