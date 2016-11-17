using UnityEngine;
using System.Collections;

public class AggroRange : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		Debug.Log("Entered");
		EventManager.Instance.TriggerEvent(new EnemySpottedEvent(gameObject));
	}
}
