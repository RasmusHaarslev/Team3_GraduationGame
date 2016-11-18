using UnityEngine;
using System.Collections;

public class AggroRange : MonoBehaviour
{

	void OnTriggerEnter(Collider col)
	{
		Debug.Log(transform.position.y + " col.y: " + col.transform.position.y);
		Debug.Log("Entered");
		if (transform.position.y < col.transform.position.y)
		{
			EventManager.Instance.TriggerEvent(new EnemySpottedEvent(gameObject.transform.parent.parent.gameObject));
		}
	}
}
