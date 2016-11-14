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

    public int shieldRange = 1;
    [Range(0, 1)]
    public float polearmStrenghtPercentageIncrease = .3f;
    [Range(0, 1)]
    public float polearmHealthPercentageIncrease = .6f;
    public int polearmRange = 3;

    [Range(0, 1)]
    public float rifleStrenghtPercentageIncrease = .3f;
    [Range(0, 1)]
    public float rifleHealthPercentageIncrease = .6f;
    public int rifleRange = 9;

    private int points = 0;
    private float strenghtIncreaseProbabLimit;
    private float healthtIncreaseProbabLimit;
    
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
                
                itemValues.range = shieldRange;
                itemValues.prefabName = "Shield";
                break;
            case EquippableitemValues.type.polearm:
                itemValues.name = "Polearm level " + level;
                
                itemValues.range = polearmRange;
                itemValues.prefabName = "Stick";
                break;
            case EquippableitemValues.type.rifle:
                itemValues.name = "Rifle level " + level;
                itemValues.damage = (int)rifleStrenghtPercentageIncrease * points;
                itemValues.health = (int)rifleHealthPercentageIncrease * points;
                itemValues.damageSpeed = (int)(1 - rifleStrenghtPercentageIncrease - rifleHealthPercentageIncrease)
                * points;
                itemValues.range = rifleRange;
                itemValues.prefabName = "Rifle";
                break;

        }
        float currentPick = 0f;
        for (int i = 0; i < points; i++)
        {
            currentPick = Random.Range(0, 1);
            if (currentPick < strenghtIncreaseProbabLimit) //increase damage
            {
                itemValues.damage += 1;
            }else if (currentPick < healthtIncreaseProbabLimit) //increase health
            {
                itemValues.health += 1;
            }
            else //increse damage speed
            {
                itemValues.damageSpeed += 1;
            }


        }

        

    }


}
