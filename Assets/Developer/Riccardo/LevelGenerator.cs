using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
	
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


    public void getCharacterByName(string characterName)
    {
        //get informations from 
        
    }


}
