using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

	NavMeshAgent agent;
	Vector3 initialPos;
	float distance;
	float distanceToLeader;
	Vector3 playerPos;
	public List<GameObject> friendlies;
	bool getNearest;
	public List<GameObject> children;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		initialPos = transform.position;
		friendlies.AddRange (GameObject.FindGameObjectsWithTag ("Friendly"));
		friendlies.Add (GameObject.FindGameObjectWithTag ("Player"));
		children.AddRange (GameObject.FindGameObjectsWithTag ("Unfriendly"));
	}
	
	// Update is called once per frame
	void Update () {
		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
		distance = Vector3.Distance (initialPos, transform.position);
		distanceToLeader = Vector3.Distance (transform.position, playerPos);

		if (distance > 8) 
		{
			agent.SetDestination (initialPos);
		}
		if (distanceToLeader > 5) {
			EventManager.Instance.TriggerEvent (new CeaseFightingEvent());
		}
	}

	public void chaseFriendly(Vector3 pos) {
		agent.SetDestination (playerPos);
		foreach (GameObject child in children) {
			child.GetComponent<EnemyMinionAI> ().GetNearestFriendly ();
		}
		EventManager.Instance.TriggerEvent (new EnemySpottedEvent (transform.position));
	}
}
