using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

public class EquippableItem : MonoBehaviour
{
    public int healthIncrease;
    public int damageIncrease;
    public float damageSpeed;
    public int range;
	public Vector3 weaponRotation;
	public Vector3 weaponPosition;

    //values gained from the database
    public EquippableitemValues itemValues;



    /// <summary>
    /// Set the equippable item values passed in the parameter
    /// </summary>
    /// <param name="initValues"></param>
    public void init(EquippableitemValues initValues)
    {
        itemValues = initValues;

        healthIncrease = itemValues.health;
        damageIncrease = itemValues.damage;
        damageSpeed = itemValues.damageSpeed;
        range = itemValues.range;
    }


}

