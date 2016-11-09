﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Character : MonoBehaviour
{
	//values gained from the database
	public CharacterValues characterBaseValues;
	//combat values
	public int health = 0;
	public int damage = 0;
	public int range = 0;
	public int damageSpeed = 0;

	public float currentHealth;

	NavMeshAgent agent;
	public GameObject target;
	GameObject targetParent;
	GameObject parent;
	public List<GameObject> currentOpponents = new List<GameObject>();

	//Combat state values
	public bool isInCombat = false;
	public bool isDead = false;
	//model values
	private Dictionary<string, Transform> slots;

	// Use this for initialization
	void Start()
	{
		slots = new Dictionary<string, Transform>(){ //TODO: chage gameObject of this list
        {"head", transform },
		{"torso", transform },
		{"leftHand", transform },
		{"rightHand", transform },

	};
	}

	void Update()
	{
		if (currentHealth <= 0)
		{
			isDead = true;
			GetComponent<MeshRenderer>().enabled = false;
		} 
	}

	void OnEnable()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();


		EventManager.Instance.StartListening<EnemySpottedEvent>(StartCombatState);
		EventManager.Instance.StartListening<TakeDamageEvent>(TakeDamage);
	}



	void OnDisable()
	{
		EventManager.Instance.StopListening<EnemySpottedEvent>(StartCombatState);
		EventManager.Instance.StopListening<TakeDamageEvent>(TakeDamage);
	}

	/// <summary>
	/// Set the character values passed in the parameter
	/// </summary>
	/// <param name="initValues"></param>
	public void init(CharacterValues initValues)
	{
		characterBaseValues = initValues;
		//setting the first summary values for the player. Those will be then increased by weapon stats when one is quipped.
		health = initValues.health;
		range = initValues.range;
		damage = initValues.damage;
		damageSpeed = initValues.damageSpeed;
		currentHealth = health;
	}

	/// <summary>
	/// Changes the stats and relocate the item on the right character slot
	/// </summary>
	/// <param name="item"></param>
	/// <param name="slot"></param>
	void equipItem(EquippableItem item, int slot = 0)
	{
		//change character parameters
		health += item.itemValues.health;
		damage += item.itemValues.damage;
		damageSpeed += item.itemValues.damageSpeed;
		if (item.itemValues.type == "hand" && slot == 1) range = item.itemValues.range; //change range only if it is the right hand
																						//parent the item to the character
		item.transform.parent = slots[item.itemValues.slot];
		item.transform.localPosition = Vector3.zero;
	}

	// Finds the appropriate target based on traits
	public void TargetOpponent()
	{
		if (currentOpponents.Count == 0)
		{
			FindCurrentOpponents();
		}

		//if (characterBaseValues.CombatFocusType == CharacterValues.combatFocusType.Nearest)
		//{
		target = FindNearestEnemy();
		//}

	}

	private void FindCurrentOpponents()
	{
		if (gameObject.tag == "Unfriendly")
		{

			if (characterBaseValues.Type == CharacterValues.type.Wolf)
			{
				List<GameObject> friendlies = new List<GameObject>();
				friendlies.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
				friendlies.Add(GameObject.FindGameObjectWithTag("Player"));

				foreach (GameObject child in friendlies)
				{
					currentOpponents.Add(child);
				}

			}
		}
		else
		{
			foreach (Transform child in targetParent.transform)
			{
				if (child.gameObject.tag == "Unfriendly")
				{
					currentOpponents.Add(child.gameObject);
				}
			}
		}
	}

	public GameObject FindNearestEnemy()
	{
		GameObject finalTarget;
		finalTarget = currentOpponents[0];
		float min = float.MaxValue;

		foreach (var possibleTarget in currentOpponents)
		{
			float distances;
			distances = Vector3.Distance(transform.position, possibleTarget.transform.position);
			if (distances < min)
			{
				min = distances;
				finalTarget = possibleTarget;
			}
		}
		Debug.Log("Final target is: " + finalTarget.name);
		return finalTarget;
	}

	private void StartCombatState(EnemySpottedEvent e)
	{
		if (!isInCombat)
		{
			targetParent = e.parent;
			TargetOpponent();
			isInCombat = true;
		}
	}

	public void DealDamage()
	{
		EventManager.Instance.TriggerEvent(new TakeDamageEvent(damage, target));
	}

	private void TakeDamage(TakeDamageEvent e)
	{
		if (e.target == gameObject)
		{
			currentHealth -= e.damage;
		}
	}
}

