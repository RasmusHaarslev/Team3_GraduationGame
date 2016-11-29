using UnityEngine;
using System.Collections;

public class RemoveOnDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{
		EventManager.Instance.StartListening<EnemyDeathEvent>(EnemyDeath);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<EnemyDeathEvent>(EnemyDeath);
	}

	void EnemyDeath(EnemyDeathEvent e)
	{
		if (e.enemy == transform.parent.gameObject)
		{
			gameObject.SetActive(false);
		}
	}
}
