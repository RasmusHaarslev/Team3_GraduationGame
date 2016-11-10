using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    private DataService dataService;

    // Use this for initialization
    void Start () {

        dataService = new DataService("tempDatabase.db");

        dataService.CreateDB();
        GameObject daniel = dataService.GenerateCharacterByName("Daniel", Vector3.zero);
        //print( dataService.GetCharacterEquippableItemsValues(daniel.GetComponent<Character>().characterBaseValues.id).ToList().Count);
		GameObject john = dataService.GenerateCharacterByName("John", Vector3.left);
		GameObject nicolai = dataService.GenerateCharacterByName("Nicolai", Vector3.right);
		GameObject peter = dataService.GenerateCharacterByName("Peter", Vector3.forward);
		//TODO acquire data from playerprefs

        //spawn the other character from the Points of Interests
        spawnEnemies();
	}
	
	// Update is called once per frame
	void Update () {
	
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
        //finds all the types of enemies and retrieves all the character tiers for those type
        PointOfInterestManager[] pointsOfInterests = GetComponentsInChildren<PointOfInterestManager>();
        PointOfInterestManager.EncounterType[] types = (from poi in pointsOfInterests select poi.type).Distinct().ToArray(); //getting distinct values!
        
        Dictionary<CharacterValues.type,GameObject[]> spawnLists = new Dictionary<CharacterValues.type, GameObject[]>();

        foreach (PointOfInterestManager.EncounterType type in types)
        {
            switch (type)
            {
              case PointOfInterestManager.EncounterType.wolf:
                    spawnLists.Add(CharacterValues.type.Wolf, dataService.GenerateCharactersByType(CharacterValues.type.Wolf).ToArray());
                    break;
              case PointOfInterestManager.EncounterType.rival:
                    dataService.GetCharactersValuesByType(CharacterValues.type.Tribesman); //TODO continue from this point!!
                    break;
            }
            
        }


        CharacterSpawner[] characterSpawns =  GetComponentsInChildren<CharacterSpawner>();
        
        for (int i = 0; i < characterSpawns.Length; i++)
        {
          dataService.GenerateCharacterByName(characterSpawns[i].characterName, characterSpawns[i].transform.position, characterSpawns[i].transform.rotation);
        }
        
    }

}
