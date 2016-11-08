using UnityEngine;
using System.Collections;

public class NaturalMovement : MonoBehaviour {

	GameObject leader;
	Vector3 newPos;
	public int id = 0;

	// Use this for initialization
	void Start ()
	{
		leader = GameObject.FindGameObjectWithTag("Player");
		if (id == 0) { 
			transform.position = leader.transform.position + transform.forward * 2;
		} else
		{
			transform.position = leader.transform.position - transform.forward * 2;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (id == 0)
		{
			transform.position = Vector3.Lerp(transform.position, leader.transform.position + leader.transform.forward * 2, Time.deltaTime * 8);
		} else
		{
			transform.position = Vector3.Lerp(transform.position, leader.transform.position - leader.transform.forward * 2, Time.deltaTime * 8);
		}
	}
}
