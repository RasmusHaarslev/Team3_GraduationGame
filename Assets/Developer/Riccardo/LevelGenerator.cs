using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class LevelGenerator : MonoBehaviour
{
    public int enemyStatsMultiplier = 1;
    public int enemyStatsAdditive = 1;
    public int difficultyLevel = 5;

	private DataService dataService;

	// Use this for initialization
	void Start()
	{
	    difficultyLevel = PlayerPrefs.GetInt(StringResources.hardnessLevel, difficultyLevel);
        
        dataService = new DataService(StringResources.databaseName);

        //dataService.CreateDB();
       
	    dataService.GetPlayerFellowshipInPosition(gameObject.GetComponentInChildren<FellowshipSpawnPoint>().transform);
        

		//TODO acquire data from playerprefs
        
        //spawn the other character from the Points of Interests
        spawnEnemies();
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

    public GameObject GenerateCharacterByName(string characterName, Vector3 position)
    {
        //get informations from database
        CharacterValues charValues = dataService.GetCharacterValuesByName(characterName);
        //load character prefab, weapons prefab and attach them
            //load prefab
            GameObject characterGameObject = (GameObject)Instantiate(Resources.Load(StringResources.characterPrefabsPath + charValues.prefabName),position,Quaternion.identity) as GameObject;
            //assign values to prefab
            characterGameObject.GetComponent<Character>().init(charValues);
            //load prefab weapons TODO handle the weapons stats
        return characterGameObject;
        
    }



    public void spawnEnemies()
    {
        //iterate trough all possible types
        PointOfInterestManager[] pointsOfInterests = GetComponentsInChildren<PointOfInterestManager>();
        PointOfInterestManager.EncounterType[] types =
            (from poi in pointsOfInterests select poi.type).Distinct().ToArray(); //getting distinct values!
        //print("Number of tiers types found " + types.Length);
        PointOfInterestManager[] currentPOIs;
        CharacterSpawner[] currentCharSpawners;
        CharacterValues[] currentTierValues = new CharacterValues[0];
        GameObject currentCharacter;
        foreach (PointOfInterestManager.EncounterType type in types)
        {

            //gather all the PointOfInterestManager of that type
            currentPOIs = (from poi in pointsOfInterests where poi.type == type select poi).ToArray();
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
            //scaling values of enemies
            ScaleCharactersValuesByLevel(currentTierValues);

            foreach (PointOfInterestManager POI in currentPOIs)
                //gets all the characters spawners and spawn the characters based on the tier
            {
                currentCharSpawners = POI.transform.GetComponentsInChildren<CharacterSpawner>();
                foreach (CharacterSpawner charSpawn in currentCharSpawners)
                {
                    
                    //create character with values based on the index of the tier [assuming that for each type, the tier will be unique, so for example we have only one wolf for wolf tier 2]
                    currentCharacter = Instantiate(
                        Resources.Load(StringResources.characterPrefabsPath +
                                       currentTierValues[charSpawn.tier - 1].prefabName),
                        //Resources.Load(StringResources.characterPrefabsPath + currentTiers[charSpawn.tier - 1].name)
                        charSpawn.transform.position, charSpawn.transform.rotation) as GameObject;
                    //assign the values ONCE it is istanced
                    currentCharacter.GetComponent<Character>().init(currentTierValues[charSpawn.tier - 1]);
                    EventManager.Instance.TriggerEvent(new EnemySpawned(currentTierValues[charSpawn.tier - 1]));
                    //parent the character to the character spawn point
                    currentCharacter.transform.parent = charSpawn.transform;
                    
                }
            }

        }
    }


    public void ScaleCharactersValuesByLevel(IEnumerable<CharacterValues> charactersValues)
    {
        foreach (CharacterValues charValues in charactersValues)
        {
            //increase values TODO all of them??
            charValues.damage = ScaleParameter(charValues.damage);
            charValues.health = ScaleParameter(charValues.health);
            

            charValues.tier = difficultyLevel + charValues.tier;
        }
    }

    /// <summary>
    /// The actual Scaling function that contains the formula
    /// </summary>
    /// <param name="values"></param>
    private int ScaleParameter(int value)
    {
        return value + (difficultyLevel*enemyStatsMultiplier) + enemyStatsAdditive;
    }

}


