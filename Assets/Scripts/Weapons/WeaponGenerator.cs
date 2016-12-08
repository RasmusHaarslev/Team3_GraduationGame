﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WeaponGenerator : MonoBehaviour
{
    [HideInInspector]
    public int newWeaponsNumber = 1;
    [Tooltip("Value to multiply to the current difficulty level in order to generate the number of points to assign to each new weapon.")]
    public int increasePointsMultiplier = 2;
    [Tooltip("Value that will be added to the (difficultyLevel * increasePointsMultiplier) in order to generate the number of points to assign to each new weapon.")]
    public int increasePointsAdder = 10;
    [Header("Shield Settings")]
    [Range(0, 1)]
    public float damagePercentageShield = .3f;
    //[Range(0, 1)]
    //public float healthPercentageShield = .6f;
    //[Range(0, 1)]
    //public float damageSpeedPercentageShield = .1f;
    public float shieldDamageSpeed = 2.5f;
    public int shieldRange = 2;

    [Header("Polearm Settings")]
    [Range(0, 1)]
    public float damagePercentagePolearm = .3f;
    //[Range(0, 1)]
    //public float healthPercentagePolearm = .1f;
    //[Range(0, 1)]
    //public float damageSpeedPercentagePolearm = .2f;
    public float polearmDamageSpeed = 1.5f;
    public int polearmRange = 2;

    [Header("Rifle Settings")]
    [Range(0, 1)]
    public float damagePercentageRifle = .3f;
    //[Range(0, 1)]
    //public float healthPercentageRifle = .4f;
    //[Range(0, 1)]
    //public float damageSpeedPercentageRifle = .1f;
    public float rifleDamageSpeed = 1.5f;
    public int rifleRange = 9;

    private int points = 0;
    private float strenghtIncreaseProbabLimit;
    private float healthtIncreaseProbabLimit;

    // Use this for initialization
    void Start()
    {
        newWeaponsNumber = PlayerPrefs.GetInt(StringResources.ItemDropAmountPrefsName, 1);
    }


    public EquippableitemValues GenerateEquippableItem(EquippableitemValues.type type, int level, float dmgProb = 0, float healthProb = 0, float dmgSpeedProb = 0)
    {
        EquippableitemValues itemValues = new EquippableitemValues();

        itemValues.Type = type;

        points = (level + 1) * increasePointsMultiplier + increasePointsAdder;
        float healthProbability = 0f; float damageProbability = 0f; float damageSpeedProbability = 0f;
        string[] modelStrings = new string[3];

        switch (type)
        {
            case EquippableitemValues.type.shield:
                //picks a random model from the shields
                modelStrings = StringResources.equipItemsModelsStrings[EquippableitemValues.type.shield] [Random.Range(0, StringResources.equipItemsModelsStrings[EquippableitemValues.type.shield].Length) ];
                itemValues.range = shieldRange;
                itemValues.damageSpeed = shieldDamageSpeed;
                itemValues.Slot = EquippableitemValues.slot.leftHand;
                //healthProbability = healthPercentageShield;
                damageProbability = damagePercentageShield;
                //damageSpeedProbability = damageSpeedPercentageShield;
                break;
            case EquippableitemValues.type.polearm:
                //picks a random model from the shields
                modelStrings = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][Random.Range(0, StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm].Length)];
                itemValues.range = polearmRange;
                itemValues.damageSpeed = polearmDamageSpeed;
                itemValues.Slot = EquippableitemValues.slot.rightHand;
                //healthProbability = healthPercentagePolearm;
                damageProbability = damagePercentagePolearm;
                //damageSpeedProbability = damageSpeedPercentagePolearm;
                break;
            case EquippableitemValues.type.rifle:
                //picks a random model from the shields
                modelStrings = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][Random.Range(0, StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle].Length)];
                itemValues.range = rifleRange;
                itemValues.damageSpeed = rifleDamageSpeed;
                itemValues.Slot = EquippableitemValues.slot.rightHand;
                //healthProbability = healthPercentageRifle;
                damageProbability = damagePercentageRifle;
                //damageSpeedProbability = damageSpeedPercentageRifle;
                break;
        }
        if (dmgProb != 0)
        {
            healthProbability = healthProb;
            damageProbability = dmgProb;
            damageSpeedProbability = dmgSpeedProb;
        }
        
        itemValues.name = modelStrings[0];
        itemValues.prefabName = modelStrings[1];
        itemValues.materialName = modelStrings[2];

        itemValues.level = level;

        float currentPick = 0f;
        //damage and damage speed have to be at least 1
        if(itemValues.damageSpeed < 1)
            itemValues.damageSpeed = 1;
        if (itemValues.damage < 1)
            itemValues.damage = 1;
        for (int i = 0; i < points; i++)
        {
            //Random.seed = (int)System.DateTime.Now.Ticks;
            currentPick = Random.Range(0.0f, 1.0f);
           
            if (currentPick < damageProbability) //increase damage
            {
                itemValues.damage += 1;
            }
            else //if (currentPick < healthProbability + damageProbability) //increase health
            {
                itemValues.health += 1;
            }
            /*
            else if (currentPick < healthProbability + damageProbability + damageSpeedProbability)//increse damage speed
            {
                itemValues.damageSpeed += 1;
            }
            */
        }

        return itemValues;

    }

    public EquippableitemValues[] GetNewItemsValues(int difficultyLevel)
    {
        EquippableitemValues[] newItemsValues = new EquippableitemValues[newWeaponsNumber];
        for (int i = 0; i < newWeaponsNumber; i++)
        {
            newItemsValues[i] = GenerateEquippableItem(RandomItemType(), difficultyLevel);
        }

        return newItemsValues;
    }

    private EquippableitemValues.type RandomItemType()
    {

        return (EquippableitemValues.type)(Random.Range(0, Enum.GetNames(typeof(EquippableitemValues.type)).Length));

    }
    /*
    private string GenerateItemName(EquippableitemValues itemValues)
    {
        String itemName = "";

        switch (itemValues.Type)
        {
            case EquippableitemValues.type.shield:
                switch (itemValues.materialName)
                {
                       // case StringResources.shield1MaterialNames[] //TODO starting from here
                }
                break;
        }

        return itemName;
    }
    */

}
