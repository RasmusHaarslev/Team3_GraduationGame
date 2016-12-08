using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class LevelGenerator : MonoBehaviour
{
    [Tooltip("Multiplier for the enemy damage and health.")]
    public float enemyStatsMultiplier = 0.25f;
    //[Tooltip("Additive for the enemy damage and health.")]
    //public float enemyStatsAdditive = 1;
    [Tooltip("How many levels a World contains.")]
    public int worldLength = 4; //how many levels are present inside a World

    [Header("Enemy Placement and Scaling")]
    [Tooltip("Parameter usually passed from the level selection. It will influence the hardness of the level.")]
    public int difficultyLevel = 5;
    [Tooltip("Number of camps that will be spawned.")]
    public int campsNumber = 4;
    [Range(0f, 1f)]
    [Tooltip("Probability before which an enemy will have the tier decreased.")]
    public float downScaleThreshold = 0.3f;
    [Range(0f, 1f)]
    [Tooltip("Probability after which an enemy will have the tier increased.")]
    public float upScaleThreshold = 0.7f;

    public bool isTutorial = false;

    private int levelStep = 0; //the local level value inside a World
    public int worldIndex = 0;

    public bool dontUseDifficultyLevel;

    private DataService dataService;

    // Use this for initialization
    void Start()
    {
        if (!dontUseDifficultyLevel) { 
            difficultyLevel = PlayerPrefs.GetInt(StringResources.hardnessLevel, difficultyLevel);
        } else {
            difficultyLevel = 1;
        }

        Debug.Log("DIFF : " + difficultyLevel);

        campsNumber = PlayerPrefs.GetInt(StringResources.TribeCampsPrefsName, campsNumber);

        levelStep = difficultyLevel % worldLength;
        worldIndex = difficultyLevel / worldLength;

        dataService = new DataService(StringResources.databaseName);

        dataService = new DataService(StringResources.databaseName);

        dataService.CreateDB();


        if (!isTutorial)
        {
            //dataService.GetPlayerFellowshipInPosition(gameObject.GetComponentInChildren<FellowshipSpawnPoint>().transform);

            int i = Random.Range(0,2);
            dataService.GetPlayerFellowshipInPosition(gameObject.GetComponentsInChildren<FellowshipSpawnPoint>()[i].transform);
        }
        //		Manager_Audio.PlaySound(Manager_Audio.musicExploreStart, this.gameObject);
        //		Manager_Audio.PlaySound(Manager_Audio.baseAmbiencePlay, this.gameObject);
        //TODO acquire data from playerprefs

        //spawn the other character from the Points of Interests
        spawnEnemies();
    }





    [ExecuteInEditMode]
    void OnValidate()
    {
        if (downScaleThreshold > upScaleThreshold)
            upScaleThreshold = downScaleThreshold + 0.05f;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="difficultyLevel">The input variable to determine the strength of characters in the scene.</param>
    /// <param name="wolfPackCount">How many Points of interests are filled with wolves.</param>
    /// <param name="tribesmanPackCount">How many Points of interests are filled with tribesmen.</param>
    /// <param name="lootCount">Quantity of loot acquired if the scene is completed.</param>
    /// <param name="environmentType">The index for the visual appearance of the environment objects on the scene</param>
    public void Init(int difficultyLevel, int wolfPackCount, int tribesmanPackCount, int lootCount, int environmentType)
    {

    }


    public void spawnEnemies()
    {
        //iterate trough all possible types
        PointOfInterestManager[] pointsOfInterests = GetComponentsInChildren<PointOfInterestManager>();

        PointOfInterestManager.EncounterType[] types =
            (from poi in pointsOfInterests select poi.type).Distinct().ToArray(); //getting distinct values!
        //print("Number of tiers types found " + types.Length);
        List<PointOfInterestManager> currentPOIs;
        CharacterSpawner[] currentCharSpawners;
        CharacterValues[] currentTierValues = new CharacterValues[0];
        GameObject currentCharacter;
        foreach (PointOfInterestManager.EncounterType type in types)
        {

            //gather all the PointOfInterestManager of that type
            currentPOIs = (from poi in pointsOfInterests where poi.type == type select poi).ToList();
            //gather all the gameobjects tiers for the corresponding CharacterValues.type, put that on an array, in ascending order
            switch (type)
            {
                case PointOfInterestManager.EncounterType.wolf:
                    //gets the wolf tiers, order them by tier ascending and put them in an array
                    currentTierValues =
                        dataService.GetCharactersValuesByType(CharacterValues.type.Wolf).OrderBy(x => x.tier).ToArray();

                    break;
                case PointOfInterestManager.EncounterType.rival:
                    //gets the wolf tiers, order them by tier ascending and put them in an array
                    currentTierValues =
                        dataService.GetCharactersValuesByType(CharacterValues.type.Tribesman)
                            .OrderBy(x => x.tier)
                            .ToArray();

                    break;

            }
            //scale POIs by level
            ScalePOIs(ref currentPOIs);

            foreach (PointOfInterestManager POI in currentPOIs)
            //gets all the characters spawners and spawn the characters based on the tier
            {
                currentCharSpawners = POI.transform.GetComponentsInChildren<CharacterSpawner>();
                foreach (CharacterSpawner charSpawn in currentCharSpawners)
                {
                    print("tier of the spawn is "+ charSpawn.tier+" current tier values length "+ currentTierValues.Length+" trying to access to index "+(charSpawn.tier - 1));
                    currentCharacter = dataService.GenerateCharacterFromValues(currentTierValues[charSpawn.tier - 1],
                        charSpawn.transform.position, charSpawn.transform.rotation);

                    EventManager.Instance.TriggerEvent(new EnemySpawned(currentTierValues[charSpawn.tier - 1]));
                    //parent the character to the character spawn point
                    currentCharacter.transform.parent = charSpawn.transform;

                }
            }

        }

        //scaling tiers and values of enemies
        ScaleCharactersValuesByLevel();
    }

    /// <summary>
    /// Scale the difficulty of each POI.
    /// Disable certain spawn based on difficulty level and other parameters set in the inspector.
    /// Randomly increases/decreases tiers based on public variables
    /// </summary>
    public void ScalePOIs(ref List<PointOfInterestManager> currentPOIs)
    {
        int minPOIEnemiesNumber = levelStep + 1;
        if (minPOIEnemiesNumber < 2)
            minPOIEnemiesNumber = 2;
        int maxPOIEnemiesNumber = Mathf.Clamp(levelStep + 2, 0, 5);

        CharacterSpawner[] currentCharSpawners = new CharacterSpawner[0];
        int currentCharSpawnersMaxIndex;
        int enemyToDisableQuantity = 0;
        int maxIndexToIncrease = 0;
        //reducing the number of camps based on the player prefs parameter
        if (campsNumber < currentPOIs.Count)
        {
            int campsToRemoveNumber = currentPOIs.Count - campsNumber;
            for (int i = 0; i < campsToRemoveNumber; i++)
            {
                print("disabling one POI");
                int indexPOIToRemove = Random.Range(0, currentPOIs.Count ); //- 1 TODO try to remove this -1! 
                currentPOIs.ElementAt(indexPOIToRemove).gameObject.SetActive(false);
                currentPOIs.RemoveAt(indexPOIToRemove);
            }
        }

        foreach (PointOfInterestManager POI in currentPOIs)
        //gets all the characters spawners and spawn the characters based on the tier
        {
            currentCharSpawners = POI.transform.GetComponentsInChildren<CharacterSpawner>(true);
            foreach (CharacterSpawner charSpawn in currentCharSpawners)
            {
                charSpawn.gameObject.SetActive(true); //enabling all the char spawns inside the POI
            }
            if (currentCharSpawners.Length >= 5)
            {
                enemyToDisableQuantity = 5 - Random.Range(minPOIEnemiesNumber, maxPOIEnemiesNumber);
                //print("between "+ (5 - minPOIEnemiesNumber)+" and "+  + "disabling " + enemyToDisableQuantity + " in a POI");
                currentCharSpawners = POI.transform.GetComponentsInChildren<CharacterSpawner>();
                currentCharSpawnersMaxIndex = currentCharSpawners.Length - 1;
                for (int i = 0; i < enemyToDisableQuantity; i++)
                {
                    currentCharSpawners[currentCharSpawnersMaxIndex - i].gameObject.SetActive(false); //TODO for now only disabling!                                                                                                     
                    //EventManager.Instance.TriggerEvent(new EnemyDeathEvent(null));
                }
                maxIndexToIncrease = currentCharSpawnersMaxIndex - enemyToDisableQuantity;
                for (int j = 0; j < maxIndexToIncrease; j++) //modifying the remaining spawn tiers
                {
                    float dice = 0;
                    dice = Random.Range(0.0f, 1.0f);

                    if (dice < downScaleThreshold)
                        currentCharSpawners[j].tier = Mathf.Clamp(currentCharSpawners[j].tier - 2, 1, 6);
                    //6 is the highest tier number
                    else if (dice > upScaleThreshold)
                        currentCharSpawners[j].tier = Mathf.Clamp(currentCharSpawners[j].tier + 2, 1, 6);
                }
            }
            else Debug.LogError("A Point of Interest was found with less than 5 character spawners!");

        }

    }

    public void ScaleCharactersValuesByLevel()
    {
        Character[] characters = GetComponentsInChildren<Character>();

        foreach (Character c in characters)
        {
            //increase values TODO all of them??
            c.damage = ScaleParameter(c.damage);
            c.health = ScaleParameter(c.health);


        }
    }

    /// <summary>
    /// The actual Scaling function that contains the formula
    /// </summary>
    /// <param name="values"></param>
    private int ScaleParameter(int value)
    {
        return (int)(value * (1 + worldIndex * enemyStatsMultiplier)); //+ enemyStatsAdditive
    }



}


