using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaposGenerator : MonoBehaviour
{
    public int increasePointsMultiplier = 2;
    public int increasePointsAdder = 10;

    [Range(0, 1)] public float shieldStrenghtPercentageIncrease = .3f;
    [Range(0, 1)]
    public float shieldHealthPercentageIncrease = .6f;

    [Range(0, 1)]
    public float polearmStrenghtPercentageIncrease = .3f;
    [Range(0, 1)]
    public float polearmHealthPercentageIncrease = .6f;

    [Range(0, 1)]
    public float rifleStrenghtPercentageIncrease = .3f;
    [Range(0, 1)]
    public float rifleHealthPercentageIncrease = .6f;

    private int points = 0;

    private DataService dataService;

    // Use this for initialization
    void Start () {

        dataService = new DataService(StringResources.databaseName);
        //calculate weapons parameters

        //....

    }


    
    public void GenerateEquippableItem(EquippableitemValues.type type, int level)
    {
        EquippableitemValues itemValues = new EquippableitemValues();
        itemValues.Type = type;

        points = level/5*increasePointsMultiplier + increasePointsAdder;

        switch (type)
        {
            case EquippableitemValues.type.shield:
                itemValues.name = "Shield level " + level;
                itemValues.damage = (int)shieldStrenghtPercentageIncrease * points;
                itemValues.health = (int)shieldHealthPercentageIncrease * points;
                itemValues.damageSpeed = (int) (1- shieldStrenghtPercentageIncrease- shieldHealthPercentageIncrease)
                *points;
                itemValues.prefabName = "Shield";
                break;
            case EquippableitemValues.type.polearm:
                itemValues.name = "Polearm level " + level;
                itemValues.damage = (int)polearmStrenghtPercentageIncrease * points;
                itemValues.health = (int)polearmHealthPercentageIncrease * points;
                itemValues.damageSpeed = (int)(1 - polearmStrenghtPercentageIncrease - polearmHealthPercentageIncrease)
                * points;
                itemValues.prefabName = "Stick";
                break;
            case EquippableitemValues.type.rifle:
                itemValues.name = "Rifle level " + level;
                itemValues.damage = (int)rifleStrenghtPercentageIncrease * points;
                itemValues.health = (int)rifleHealthPercentageIncrease * points;
                itemValues.damageSpeed = (int)(1 - rifleStrenghtPercentageIncrease - rifleHealthPercentageIncrease)
                * points;
                itemValues.prefabName = "Rifle";
                break;

        }

        

    }


}
