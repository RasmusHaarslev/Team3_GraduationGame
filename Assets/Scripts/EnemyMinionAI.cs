using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMinionAI : MonoBehaviour
{

	NavMeshAgent agent;
	Vector3 initialPos;
	float distance;
	public List<float> distancesToFriendly;
	public Vector3 nearestFriendlyPosition;
	GameObject leader;
	public List<GameObject> friendlies;
	public int health = 5;

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
		leader = GameObject.Find("EnemyLeader");
		if (leader == null)
		{
			agent.SetDestination(new Vector3(0, 0, 0));
		}
		else
		{
			distance = Vector3.Distance(initialPos, transform.position);
			if (distance > 10)
			{
				agent.SetDestination(initialPos);
			}
		}

		if (health <= 0)
		{
			gameObject.SetActive(false);
			EventManager.Instance.TriggerEvent(new EnemyDeathEvent());
		}
		if (health < 2)
		{
			agent.SetDestination(new Vector3(0, 0, 0));
		}

	}

	public void GetNearestFriendly ()
	{
		float min = float.MaxValue;
		for (int i = 0; i < friendlies.Count; i++) {
			distancesToFriendly [i] = Vector3.Distance (transform.position, friendlies [i].transform.position);
			if(distancesToFriendly [i] < min) {
                min = distancesToFriendly[i];
				nearestFriendlyPosition = friendlies [i].transform.position;
            }
        }
		agent.SetDestination (nearestFriendlyPosition);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "FriendlyWeapon")
		{
			health--;
		}
	}
}
