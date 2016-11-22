using UnityEngine;
using System.Collections;

public class TutorialAggroRange : MonoBehaviour
{
	public float verticalTriggerOffset = 1.5f;
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Friendly")
		{
			Debug.Log("parent: " + gameObject.transform.parent.parent.parent.gameObject);
			if ((col.transform.position.y - transform.position.y) < verticalTriggerOffset)
			{
				if (!transform.parent.GetComponent<TutorialCharacter>().isDead)
				{
					EventManager.Instance.TriggerEvent(new EnemySpottedEvent(gameObject.transform.parent.parent.parent.gameObject));
				}
			}
		}
	}
}
