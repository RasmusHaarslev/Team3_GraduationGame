using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        for (int i = 0; i<5; i++)
        {
            CharacterGenerator charGen = new CharacterGenerator();
            var characterValues = charGen.GenerateNewHunterValues();

            CharacterController.SaveCharacters(new List<CharacterValues>() { characterValues });
        }
    }
}
