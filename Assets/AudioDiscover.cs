using UnityEngine;
using System.Collections;
using System;

public class AudioDiscover : MonoBehaviour {

	void OnEnable()
	{
		EventManager.Instance.StartListening<AllyDeathEvent>(PlayAllyDeath);
	}
	void OnDisable()
	{
		EventManager.Instance.StopListening<AllyDeathEvent>(PlayAllyDeath);
	}

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	private void PlayAllyDeath(AllyDeathEvent e)
	{
		Manager_Audio.PlaySound(Manager_Audio.friendlyDeath, gameObject);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Unfriendly")
		{
			Manager_Audio.PlaySound(Manager_Audio.discoverEnemy, gameObject);
		} else if (col.gameObject.tag == "Friendly") // TODO: change tag to friendly encounter when it exists
		{
			Manager_Audio.PlaySound(Manager_Audio.discoverFriendly, gameObject);
		}
	}
}
