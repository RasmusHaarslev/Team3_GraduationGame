using UnityEngine;
using System.Collections;
using System;

public class AudioDiscover : MonoBehaviour
{
	public float enemyCounter;
	public float maxTimeBetweenEnemySightings = 3f;

	public float friendlyCounter;
	public float maxTimeBetweenFriendlySightings = 3f;

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
			if (enemyCounter <= 0)
			{
				enemyCounter = maxTimeBetweenEnemySightings;
				Manager_Audio.PlaySound(Manager_Audio.discoverEnemy, gameObject);
			}
		}
		else if (col.gameObject.tag == "Friendly") // TODO: change tag to friendly encounter when it exists
		{
			if (enemyCounter <= 0)
			{
				enemyCounter = maxTimeBetweenEnemySightings;
				Manager_Audio.PlaySound(Manager_Audio.discoverFriendly, gameObject);
			}
		}
	}

	void Update()
	{
		if(enemyCounter >= 0)
		{
			enemyCounter -= Time.deltaTime;
		}

		if (friendlyCounter >= 0)
		{
			friendlyCounter -= Time.deltaTime;
		}
	}
}
