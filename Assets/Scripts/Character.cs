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
    
    //Combat state values
    private bool isInCombat = false;
    //model values
    private Dictionary<string, Transform> slots;



    // Use this for initialization
    void Start()
    {
		agent.GetComponent<NavMeshAgent>();
		if (agent == null)
		{
			Debug.Log(gameObject.name + " is missing a NavMeshAgent");
		}
        slots = new Dictionary<string, Transform>(){ //TODO: chage gameObject of this list
        {"head", transform },
        {"torso", transform },
        {"leftHand", transform },
        {"rightHand", transform },
        {"legs", transform }
    };

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
        /*
        Type typeB = initValues.GetType();
        foreach (PropertyInfo property in GetType().GetProperties())
        {
            if (!property.CanRead || (property.GetIndexParameters().Length > 0))
                continue;

            PropertyInfo other = typeB.GetProperty(property.Name);
            if ((other != null) && (other.CanWrite))
                other.SetValue(initValues, property.GetValue(this, null), null);
        }
        */
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

	public void TargetOpponent()
	{



	}



}
