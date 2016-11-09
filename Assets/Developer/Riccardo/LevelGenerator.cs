using UnityEngine;
using System.Collections;
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
        print( dataService.GetCharacterEquippableItemsValues(daniel.GetComponent<Character>().characterBaseValues.id).ToList().Count);
		GameObject john = dataService.GenerateCharacterByName("John", Vector3.left);
		GameObject nicolai = dataService.GenerateCharacterByName("Nicolai", Vector3.right);
		GameObject peter = dataService.GenerateCharacterByName("Peter", Vector3.forward);
		//TODO acquire data from playerprefs


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


}
