using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PointOfInterestManager : MonoBehaviour
{

	public EncounterType type;
	[Range(3, 20)]
	public int radius = 8;

	public enum EncounterType
	{
		choice,
		rival,
		wolf
	}


	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);

	}

	// Use this for initialization
	void Start()
	{

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

	public float GetAverageCharactersHealth()
	{
		List<Character> characterList = transform.GetComponentsInChildren<Character>().ToList();
		float s = 0;
		float averageHealth = 0;
		foreach (Character character in characterList)
		{
			var characterCurrentHealth = character.currentHealth;
			if (characterCurrentHealth < 0)
			{
				character.currentHealth = 0;
			}
			s += character.currentHealth;
		}
		averageHealth = s / characterList.Count;
		return averageHealth;
	}
}
