using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMinionAI : MonoBehaviour
{

	NavMeshAgent agent;
	Vector3 initialPos;
	float distance;
	public List<float> distancesToFriendly;
	Vector3 nearestFriendlyPosition;
	GameObject leader;
	public List<GameObject> friendlies;

	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
		initialPos = transform.position;
		leader = GameObject.Find ("EnemyLeader");
		friendlies = leader.GetComponent<EnemyAI> ().friendlies;
		for (int i = 0; i < 5; i++) {
			distancesToFriendly.Add (float.MaxValue);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		distance = Vector3.Distance (initialPos, transform.position);
		if (distance > 20) 
		{
			agent.SetDestination (initialPos);
		}
	}

	public void GetNearestFriendly ()
	{
		float min = float.MaxValue;
		for (int i = 0; i < friendlies.Count; i++) {
			distancesToFriendly [i] = Vector3.Distance (transform.position, friendlies [i].transform.position);
			if(distancesToFriendly [i] < min)
				nearestFriendlyPosition = friendlies [i].transform.position;
		}
		Debug.Log ("nearest");
		agent.SetDestination (nearestFriendlyPosition);
	}
}
