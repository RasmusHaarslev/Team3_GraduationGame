using UnityEngine;
using System.Collections;
using System;

namespace AIns.FSM
{
	public class PrototypeAI : CoroutineMachine
	{

		NavMeshAgent agent;
		GameObject leader;
		public int index;
		public int health = 8;
		public bool enemySpotted = false;
		Vector3 enemyPosition;
		bool attack = false;
		public int numberOfEnemies = 5;

		// Update is called once per frame
		void Update()
		{
			if (health <= 0)
			{
				gameObject.SetActive(false);
			}
		}

		void OnEnable()
		{
			EventManager.Instance.StartListening<AttackStateEvent>(Attack);
			EventManager.Instance.StartListening<DefendStateEvent>(Defend);
			EventManager.Instance.StartListening<EnemySpottedEvent>(EnemySpotted);
			EventManager.Instance.StartListening<CeaseFightingEvent>(CeaseFighting);
			EventManager.Instance.StartListening<EnemyDeathEvent>(DecreaseEnemyCount);
			agent = GetComponent<NavMeshAgent>();
			leader = GameObject.FindGameObjectWithTag("Player");
		}

		void OnDestroy()
		{
			EventManager.Instance.StopListening<AttackStateEvent>(Attack);
			EventManager.Instance.StopListening<DefendStateEvent>(Defend);
			EventManager.Instance.StopListening<EnemySpottedEvent>(EnemySpotted);
			EventManager.Instance.StopListening<CeaseFightingEvent>(CeaseFighting);
			EventManager.Instance.StopListening<EnemyDeathEvent>(DecreaseEnemyCount);
		}

		private void DecreaseEnemyCount(EnemyDeathEvent e)
		{
			numberOfEnemies--;
		}

		private void Defend(DefendStateEvent e)
		{
			attack = false;
		}

		private void Attack(AttackStateEvent e)
		{
			attack = true;
		}

		void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.tag == "EnemyWeapon")
			{
				health--;
			}
		}

		protected override StateRoutine InitialState
		{

			get
			{
				return StartState;
			}
		}

		IEnumerator StartState()
		{
			if (numberOfEnemies <= 0)
			{
				enemySpotted = false;
			}
			if (enemySpotted)
			{
				yield return new TransitionTo(ChaseState, DefaultTransition);
			}
			yield return new TransitionTo(FollowState, DefaultTransition);
		}

		IEnumerator ChaseState()
		{
			if (attack)
			{
				agent.stoppingDistance = 0;
				agent.SetDestination(enemyPosition);
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
			if (index == 0)
			{
				agent.SetDestination(leader.transform.position - leader.transform.right * 2);
			}
			else if (index == 1)
			{
				agent.SetDestination(leader.transform.position + leader.transform.right * 2);
			}
			else if (index == 2)
			{
				agent.SetDestination(leader.transform.position + leader.transform.forward * 2);
			}
			else if (index == 3)
			{
				agent.SetDestination(leader.transform.position - leader.transform.forward * 2);
			}
			yield return new TransitionTo(StartState, DefaultTransition);
		}

		IEnumerator CombatState()
		{
			if (attack)
			{
				agent.SetDestination(enemyPosition);
			}
			else
			{

			}

			yield return new TransitionTo(StartState, DefaultTransition);
		}

		IEnumerator DefaultTransition(StateRoutine from, StateRoutine to)
		{

			yield return new WaitForSeconds(0.05f);
		}

		void EnemySpotted(EnemySpottedEvent e)
		{
			enemyPosition = e.pos;
			enemySpotted = true;
		}

		void CeaseFighting(CeaseFightingEvent e)
		{
			enemySpotted = false;
		}
	}
}
