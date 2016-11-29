using UnityEngine;
using System.Collections;

public class AggroRange : MonoBehaviour
{
	public float verticalTriggerOffset = 1.0f;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Friendly")
		{
			if ((col.transform.position.y - transform.position.y) < verticalTriggerOffset)
			{
				if (!transform.parent.GetComponent<Character>().isDead && !col.GetComponent<Character>().isFleeing)
					EventManager.Instance.TriggerEvent(new EnemySpottedEvent(gameObject.transform.parent.parent.parent.gameObject));
			}
		}
	}
}
