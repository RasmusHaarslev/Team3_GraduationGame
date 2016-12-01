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
        if (CharacterController.CharactersLoaded.Count == 0)
        {
            CharacterController.LoadCharacters();
        }

        for (int i = 0; i < 3; i++)
        {
            CharacterGenerator charGen = new CharacterGenerator();
            var characterValues = charGen.GenerateNewHunterValues();
            CharacterController.CharactersLoaded.Add(characterValues);
        }

        if (ItemController.ItemsLoaded.Count == 0)
        {
            ItemController.LoadItems();
        }

        for (int i = 0; i < 3; i++)
        {
            WeaponGenerator charGen = new WeaponGenerator();
            var itemValues = charGen.GenerateEquippableItem(EquippableitemValues.type.polearm, 0);
            itemValues.characterId = itemValues.characterId = CharacterController.CharactersLoaded.Count;
            ItemController.ItemsLoaded.Add(itemValues);
        }

        ItemController.SaveItem(ItemController.ItemsLoaded );
        CharacterController.SaveCharacters(CharacterController.CharactersLoaded );
    }
}
