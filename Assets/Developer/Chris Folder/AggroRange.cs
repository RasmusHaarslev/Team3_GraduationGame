using UnityEngine;
using System.Collections;

public class AggroRange : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		EventManager.Instance.TriggerEvent(new EnemySpottedEvent(gameObject));
	}

	void OnTriggerExit(Collider col)
	{
		
	}
}
