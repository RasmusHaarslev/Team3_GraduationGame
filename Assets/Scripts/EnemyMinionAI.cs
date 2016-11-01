using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMinionAI : MonoBehaviour
{

	NavMeshAgent agent;
	Vector3 initialPos;
	float distance;
	float minDistanceToFriendly;
	Vector3 nearestFriendlyPosition;
	GameObject leader;
	List<GameObject> friendlies;

	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
		initialPos = transform.position;
		leader = GameObject.Find ("EnemyLeader");
		friendlies = leader.GetComponent<EnemyAI> ().friendlies;
		minDistanceToFriendly = float.MaxValue;
	}

	// Update is called once per frame
	void Update ()
	{
		distance = Vector3.Distance (initialPos, transform.position);
		if (distance > 8) {
			agent.SetDestination (initialPos);
		}
	}

	public void GetNearestFriendly ()
	{
		foreach (GameObject g in friendlies) {
			if (minDistanceToFriendly > Vector3.Distance (transform.position, g.transform.position)) {
				minDistanceToFriendly = Vector3.Distance (transform.position, g.transform.position);
				nearestFriendlyPosition = g.transform.position;
			}
		}
	}
}
