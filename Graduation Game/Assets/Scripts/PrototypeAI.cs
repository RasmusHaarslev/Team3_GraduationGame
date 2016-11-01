using UnityEngine;
using System.Collections;


namespace AIns.FSM
{
	public class PrototypeAI : CoroutineMachine
	{

		NavMeshAgent agent;
		GameObject leader;
		public int index;
		public bool enemySpotted = false;
		Vector3 enemyPosition;



		// Update is called once per frame
		void Update ()
		{		
		
		}

		void OnEnable ()
		{
			EventManager.Instance.StartListening<EnemySpottedEvent> (EnemySpotted);
			EventManager.Instance.StartListening<CeaseFightingEvent> (CeaseFighting);
			agent = GetComponent<NavMeshAgent> ();
			leader = GameObject.FindGameObjectWithTag ("Player");
		}

		void OnDestroy ()
		{
			EventManager.Instance.StopListening<EnemySpottedEvent> (EnemySpotted);
			EventManager.Instance.StopListening<CeaseFightingEvent> (CeaseFighting);
		}

		protected override StateRoutine InitialState {

			get {
				return StartState;
			}
		}

		IEnumerator StartState ()
		{
			if (enemySpotted) {
				yield return new TransitionTo (ChaseState, DefaultTransition);
			}

			yield return new TransitionTo (FollowState, DefaultTransition);
		}

		IEnumerator ChaseState ()
		{
			agent.SetDestination (enemyPosition);

			yield return new TransitionTo (StartState, DefaultTransition);
		}

		IEnumerator FollowState ()
		{

			if (index == 0) {
				agent.SetDestination (leader.transform.position + Vector3.left + Vector3.left);
			} else if (index == 1) {
				agent.SetDestination (leader.transform.position + Vector3.right + Vector3.right);
			} else if (index == 2) {
				agent.SetDestination (leader.transform.position + Vector3.forward + Vector3.forward);
			} else if (index == 3) {
				agent.SetDestination (leader.transform.position + Vector3.back + Vector3.back);
			}
			yield return new TransitionTo (StartState, DefaultTransition);
		}

		IEnumerator CombatState ()
		{
			
			agent.SetDestination (enemyPosition);

			yield return new TransitionTo (StartState, DefaultTransition);
		}

		IEnumerator DefaultTransition (StateRoutine from, StateRoutine to)
		{
	
			yield return new WaitForSeconds (0.05f);
		}

		void EnemySpotted (EnemySpottedEvent e)
		{
			enemyPosition = e.pos;
			enemySpotted = true;
		}

		void CeaseFighting (CeaseFightingEvent e) {
			enemySpotted = false;
		}
	}
}
