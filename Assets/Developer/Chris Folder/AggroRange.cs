using UnityEngine;
using System.Collections;

public class AggroRange : MonoBehaviour
{
	public float verticalTriggerOffset = 1.5f;
	void OnTriggerEnter(Collider col)
	{
		Debug.Log(col.gameObject.tag);
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Friendly")
		{
			Debug.Log("saw player");
			if ((col.transform.position.y - transform.position.y) < verticalTriggerOffset)
			{
				EventManager.Instance.TriggerEvent(new EnemySpottedEvent(gameObject.transform.parent.parent.parent.gameObject));
			}
		}
	}
}
