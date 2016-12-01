﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WeaponGenerator : MonoBehaviour
{
    public int newWeaponsNumber = 1;
    public int increasePointsMultiplier = 2;
    public int increasePointsAdder = 10;
    [Header("Shield Settings")]
    [Range(0, 1)]
    public float damagePercentageShield = .3f;
    [Range(0, 1)]
    public float healthPercentageShield = .6f;
    [Range(0, 1)]
    public float damageSpeedPercentageShield = .1f;
    public int shieldRange = 1;

    [Header("Polearm Settings")]
    [Range(0, 1)]
    public float damagePercentagePolearm = .3f;
    [Range(0, 1)]
    public float healthPercentagePolearm = .1f;
    [Range(0, 1)]
    public float damageSpeedPercentagePolearm = .2f;
    public int polearmRange = 3;

    [Header("Rifle Settings")]
    [Range(0, 1)]
    public float damagePercentageRifle = .3f;
    [Range(0, 1)]
    public float healthPercentageRifle = .4f;
    [Range(0, 1)]
    public float damageSpeedPercentageRifle = .1f;
    public int rifleRange = 9;

    private int points = 0;
    private float strenghtIncreaseProbabLimit;
    private float healthtIncreaseProbabLimit;

    //private DataService dataService;
    [ExecuteInEditMode]
    void OnValidate()
    {//adjusting probability values among the item types if some value has been changed in the editor
        if (damagePercentageShield + healthPercentageShield + damageSpeedPercentageShield > 1)
        {
            damagePercentageShield = Mathf.Clamp01(damagePercentageShield - 0.02f);
            healthPercentageShield = Mathf.Clamp01(healthPercentageShield - 0.02f);
            damageSpeedPercentageShield = Mathf.Clamp01(damageSpeedPercentageShield - 0.02f);
        }
        if (damagePercentagePolearm + healthPercentagePolearm + damageSpeedPercentagePolearm > 1)
        {
            damagePercentagePolearm = Mathf.Clamp01(damagePercentagePolearm - 0.02f);
            healthPercentagePolearm = Mathf.Clamp01(healthPercentagePolearm - 0.02f);
            damageSpeedPercentagePolearm = Mathf.Clamp01(damageSpeedPercentagePolearm - 0.02f);
        }
        if (damagePercentageRifle + healthPercentageRifle + damageSpeedPercentageRifle > 1)
        {
            damagePercentageRifle = Mathf.Clamp01(damagePercentageRifle - 0.02f);
            healthPercentageRifle = Mathf.Clamp01(healthPercentageRifle - 0.02f);
            damageSpeedPercentageRifle = Mathf.Clamp01(damageSpeedPercentageRifle - 0.02f);
        }

    }


    // Use this for initialization
    void Start()
    {

        //dataService = new DataService(StringResources.databaseName);
        //calculate weapons parameters

        //....

    }


    public EquippableitemValues GenerateEquippableItem(EquippableitemValues.type type, int level, float dmgProb = 0, float healthProb = 0, float dmgSpeedProb = 0)
    {
        EquippableitemValues itemValues = new EquippableitemValues();

        itemValues.Type = type;

        points = (level / 5 + 1) * increasePointsMultiplier + increasePointsAdder;
        float healthProbability = 0f; float damageProbability = 0f; float damageSpeedProbability = 0f;
        string[] modelStrings = new string[3];

        switch (type)
        {
            case EquippableitemValues.type.shield:
                //picks a random model from the shields
                modelStrings = StringResources.equipItemsModelsStrings[EquippableitemValues.type.shield] [Random.Range(0, StringResources.equipItemsModelsStrings[EquippableitemValues.type.shield].Length - 1) ];
                itemValues.range = shieldRange;
                itemValues.Slot = EquippableitemValues.slot.leftHand;
                healthProbability = healthPercentageShield;
                damageProbability = damagePercentageShield;
                damageSpeedProbability = damageSpeedPercentageShield;
                break;
            case EquippableitemValues.type.polearm:
                //picks a random model from the shields
                modelStrings = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][Random.Range(0, StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm].Length - 1)];
                itemValues.range = polearmRange;
                itemValues.Slot = EquippableitemValues.slot.rightHand;
                healthProbability = healthPercentagePolearm;
                damageProbability = damagePercentagePolearm;
                damageSpeedProbability = damageSpeedPercentagePolearm;
                break;
            case EquippableitemValues.type.rifle:
                //picks a random model from the shields
                modelStrings = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][Random.Range(0, StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle].Length - 1)];
                itemValues.range = rifleRange;
                itemValues.Slot = EquippableitemValues.slot.rightHand;
                healthProbability = healthPercentageRifle;
                damageProbability = damagePercentageRifle;
                damageSpeedProbability = damageSpeedPercentageRifle;
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
        itemValues.damageSpeed = 1;
        itemValues.damage = 1;
        for (int i = 0; i < points; i++)
        {
            //Random.seed = (int)System.DateTime.Now.Ticks;
            currentPick = Random.Range(0.0f, 1.0f);
           
            if (currentPick < damageProbability) //increase damage
            {
                itemValues.damage += 1;
            }
            else if (currentPick < healthProbability + damageProbability) //increase health
            {
                itemValues.health += 1;
            }
            else if (currentPick < healthProbability + damageProbability + damageSpeedProbability)//increse damage speed
            {
                itemValues.damageSpeed += 1;
            }

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
