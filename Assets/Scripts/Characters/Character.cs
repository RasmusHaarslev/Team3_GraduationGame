using System;
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

	NavMeshAgent agent;
	GameObject target;
	GameObject parent;
    
    //Combat state values
    public bool isInCombat = false;
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

	void OnEnable()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();


		EventManager.Instance.StartListening<EnemySpottedEvent>(StartCombat);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<EnemySpottedEvent>(StartCombat);
	}

	// Update is called once per frame
	void Update()
    {

    }

    /// <summary>
    /// Set the character values passed in the parameter
    /// </summary>
    /// <param name="initValues"></param>
    public void init(CharacterValues initValues)
    {
        characterBaseValues = initValues;
        
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
		if (characterBaseValues.CombatFocusType == CharacterValues.combatFocusType.Nearest)
		{
			target = FindNearestEnemy();
		}

	}

	private GameObject FindNearestEnemy()
	{
		return gameObject;
	}

	private void StartCombat(EnemySpottedEvent e)
	{
		isInCombat = true;
	}

}
