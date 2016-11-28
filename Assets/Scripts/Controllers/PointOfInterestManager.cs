using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PointOfInterestManager : MonoBehaviour
{
	public float originalAverageHealth = 0;
	public EncounterType type;
	[Range(3, 20)]
	public int radius = 8;
	private int EnemiesAlive = 0;
	private LevelGenerator levelGenerator;
	public List<GameObject> enemies = new List<GameObject>();
	private bool isEnemiesSet = false;
	public enum EncounterType
	{
		choice,
		rival,
		wolf
	}

	void OnEnable()
	{
		EventManager.Instance.StartListening<EnemyDeathEvent>(EnemyDeath);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<EnemyDeathEvent>(EnemyDeath);
	}

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	private void EnemyDeath(EnemyDeathEvent e)
	{
		if (enemies.Contains(e.enemy))
		{
			enemies.Remove(e.enemy);
			if (enemies.Count == 0)
			{
				EventManager.Instance.TriggerEvent(new ClearedCampEvent());
			}

		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);

	}

	// Use this for initialization
	void Start()
	{
		levelGenerator = UnityEngine.Object.FindObjectOfType<LevelGenerator>();
		//activate the point of interest decoraction
		switch (type)
		{
			case EncounterType.choice:
				transform.Find("ChoiceDecor").gameObject.SetActive(true);
				break;
			case EncounterType.rival:
				transform.Find("RivalDecor").gameObject.SetActive(true);
				break;
			case EncounterType.wolf:
				transform.Find("WolfDecor").gameObject.SetActive(true);
				break;
		}
	}

	void Update()
	{
		if (!isEnemiesSet)
		{
			foreach (Transform child in transform)
			{
				foreach (Transform childchild in child)
				{
					if (childchild.tag == "Unfriendly")
					{
						enemies.Add(childchild.gameObject);
					}
				}
			}
			isEnemiesSet = true;
		}
	}

	public float GetAverageCharactersHealth()
	{
		if (levelGenerator == null)
		{
			levelGenerator = UnityEngine.Object.FindObjectOfType<LevelGenerator>();
		}
		float averageHealth = 0;
		if (levelGenerator.isTutorial)
		{
			List<TutorialCharacter> characterList = transform.GetComponentsInChildren<TutorialCharacter>().ToList();
			float s = 0;
			foreach (var character in characterList)
			{
				var characterCurrentHealth = character.currentHealth;
				if (characterCurrentHealth < 0)
				{
					character.currentHealth = 0;
				}
				s += character.currentHealth;
			}
			averageHealth = s / characterList.Count;
			if (originalAverageHealth == 0)
			{
				originalAverageHealth = averageHealth;
			}
		}
		else
		{
			List<Character> characterList = transform.GetComponentsInChildren<Character>().ToList();
			float s = 0;
			foreach (var character in characterList)
			{
				var characterCurrentHealth = character.currentHealth;
				if (characterCurrentHealth < 0)
				{
					character.currentHealth = 0;
				}
				s += character.currentHealth;
			}
			averageHealth = s / characterList.Count;
			if (originalAverageHealth == 0)
			{
				originalAverageHealth = averageHealth;
			}
		}

		return averageHealth;
	}
}
