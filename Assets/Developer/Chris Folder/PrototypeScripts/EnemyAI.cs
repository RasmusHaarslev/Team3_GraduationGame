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
	public int health = 8;

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
		if (health <= 0)
		{
			gameObject.SetActive(false);
			EventManager.Instance.TriggerEvent(new EnemyDeathEvent());
		}
		if (health < 2)
		{
			agent.SetDestination(new Vector3(0, 0, 0));
		}
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

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "FriendlyWeapon")
		{
			health--;
		}
	}

	public void chaseFriendly(Vector3 pos) {
		
		foreach (GameObject child in children) {
			child.GetComponent<EnemyMinionAI> ().GetNearestFriendly ();
		}
		agent.SetDestination (playerPos);
		//EventManager.Instance.TriggerEvent (new EnemySpottedEvent ());
	}
}
