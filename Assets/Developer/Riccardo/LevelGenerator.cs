using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class LevelGenerator : MonoBehaviour
{
	private DataService dataService;

	// Use this for initialization
	void Start()
	{

		dataService = new DataService("tempDatabase.db");

		dataService.CreateDB();
		GameObject daniel = dataService.GenerateCharacterByName("Daniel", new Vector3(-30, 45, -45));
		//print( dataService.GetCharacterEquippableItemsValues(daniel.GetComponent<Character>().characterBaseValues.id).ToList().Count);
		GameObject john = dataService.GenerateCharacterByName("John",new Vector3(-33, 45, -45));
		GameObject nicolai = dataService.GenerateCharacterByName("Nicolai", new Vector3(-32, 45, -45));
		GameObject peter = dataService.GenerateCharacterByName("Peter", new Vector3(-31, 45, -45));

		GameObject Yasmin1 = dataService.GenerateCharacterByName("Yasmin", GameObject.FindGameObjectWithTag("EnemyParent").transform.position);
		GameObject Yasmin2 = dataService.GenerateCharacterByName("Yasmin", GameObject.FindGameObjectWithTag("EnemyParent").transform.position);
		GameObject Yasmin3 = dataService.GenerateCharacterByName("Yasmin", GameObject.FindGameObjectWithTag("EnemyParent").transform.position);

		Yasmin1.transform.parent = GameObject.FindGameObjectWithTag("EnemyParent").transform;
		Yasmin2.transform.parent = GameObject.FindGameObjectWithTag("EnemyParent").transform;
		Yasmin3.transform.parent = GameObject.FindGameObjectWithTag("EnemyParent").transform;


<<<<<<< HEAD
        dataService = new DataService(StringResources.databaseName);
=======
>>>>>>> 8c5e29ac0e3a4292ba527a136a6d1d4dee359f78

		//TODO acquire data from playerprefs
<<<<<<< HEAD
        
        //spawn the other character from the Points of Interests
        spawnEnemies();
	}
	
	// Update is called once per frame
	void Update () {
	
=======

		//spawn the other character from the Points of Interests
		spawnEnemies();
>>>>>>> 8c5e29ac0e3a4292ba527a136a6d1d4dee359f78
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
        /*
        //finds all the types of enemies and retrieves all the character tiers for those type
        PointOfInterestManager[] pointsOfInterests = GetComponentsInChildren<PointOfInterestManager>();
        PointOfInterestManager.EncounterType[] types = (from poi in pointsOfInterests select poi.type).Distinct().ToArray(); //getting distinct values!
        
        Dictionary<CharacterValues.type,GameObject[]> spawnLists = new Dictionary<CharacterValues.type, GameObject[]>();

        //loading all the characters in the types that are present in the scene
        foreach (PointOfInterestManager.EncounterType type in types)
        {
            switch (type)
            {
              case PointOfInterestManager.EncounterType.wolf: //gets the wolf tiers, order them by tier ascending and put them in an array
                    spawnLists.Add(CharacterValues.type.Wolf, dataService.GenerateCharactersByType(CharacterValues.type.Wolf).OrderBy(x => x.GetComponent<CharacterValues>().tier).ToArray());
                    break;
              case PointOfInterestManager.EncounterType.rival: //gets the tribesman tier, order them by tier ascending and put them in an array
                    dataService.GetCharactersValuesByType(CharacterValues.type.Tribesman); //TODO continue from this point!!
                    break;
            }
            
        }
        */

        //iterate trough all possible types
        PointOfInterestManager[] pointsOfInterests = GetComponentsInChildren<PointOfInterestManager>();
        PointOfInterestManager.EncounterType[] types = (from poi in pointsOfInterests select poi.type).Distinct().ToArray(); //getting distinct values!
        print("Number of tiers types found " + types.Length);
        PointOfInterestManager[] currentPOIs;
        CharacterSpawner[] currentCharSpawners;
        GameObject[] currentTiers;
        foreach (PointOfInterestManager.EncounterType type in types) {
            
            //gather all the PointOfInterestManager of that type
            currentPOIs = (from poi in pointsOfInterests where poi.type == type select poi).ToArray();
            //gather all the gameobjects tiers for the corresponding CharacterValues.type, put that on an array, in ascending order
            switch (type)
            {
                case PointOfInterestManager.EncounterType.wolf://gets the wolf tiers, order them by tier ascending and put them in an array
                    currentTiers = dataService.GenerateCharactersByType(CharacterValues.type.Wolf).OrderBy(x => x.GetComponent<CharacterValues>().tier).ToArray();
                    print("Number of tiers for wolf "+currentTiers.Length);
                    foreach (PointOfInterestManager POI in currentPOIs) //gets all the characters spawners and spawn the characters based on the tier
                    {
                        currentCharSpawners = POI.transform.GetComponentsInChildren<CharacterSpawner>();
                        print("Number of spawns for current POI" + currentCharSpawners.Length);
                        foreach (CharacterSpawner charSpawn in currentCharSpawners)
                        {
                            //create character with values based on the index of the tier [assuming that for each type, the tier will be unique, so for example we have only one wolf for wolf tier 2]
                            Instantiate(
                                Resources.Load(StringResources.characterPrefabsPath + currentTiers[charSpawn.tier - 1]),
                                charSpawn.transform.position, Quaternion.identity);
                        }
                    }

                    break;
            }
            
        }

        /*
        CharacterSpawner[] characterSpawns =  GetComponentsInChildren<CharacterSpawner>();
        
        for (int i = 0; i < characterSpawns.Length; i++)
        {
          dataService.GenerateCharacterByName(characterSpawns[i].characterName, characterSpawns[i].transform.position, characterSpawns[i].transform.rotation);
        }
        */
    }

}
