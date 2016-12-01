using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterGenerator
{
    public int newCharPoints = 15;
    public float damagePointsChance = 0.5f;

    public CharacterValues GenerateNewHunterValues(List<CharacterValues> newCharacterSoldierList = null, int points = 0, float strenghtProbab = 0)
    {
        if (points != 0)
            newCharPoints = points;
        if (strenghtProbab != 0)
            damagePointsChance = strenghtProbab;

        CharacterValues newCharValues = new CharacterValues();
        //generate prefab 
        newCharValues.prefabName = StringResources.follower1PrefabName;
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        //generate random name
        string characterName;
        if (randomValue > 0.5f)//we create a Male
        {
            characterName = StringResources.maleNames[UnityEngine.Random.Range(0, StringResources.maleNames.Length)];
            newCharValues.isMale = true;
            newCharValues.materialName = StringResources.maleHuntersMaterials[UnityEngine.Random.Range(0, StringResources.maleHuntersMaterials.Length)];
        }
        else //we create a female
        {
            characterName = StringResources.femaleNames[UnityEngine.Random.Range(0, StringResources.femaleNames.Length)];
            newCharValues.isMale = false;
            newCharValues.materialName = StringResources.femaleHuntersMaterials[UnityEngine.Random.Range(0, StringResources.femaleHuntersMaterials.Length)];
        }

        //give name to soldier and assign type
        newCharValues.name = characterName;
        newCharValues.Type = CharacterValues.type.Hunter;

        Array values = Enum.GetValues(typeof(CharacterValues.CombatTrait));
        newCharValues.combatTrait = (CharacterValues.CombatTrait)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        values = Enum.GetValues(typeof(CharacterValues.TargetTrait));
        newCharValues.targetTrait = (CharacterValues.TargetTrait)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        //generate stats
        newCharValues = GenerateNewCharacterStats(newCharValues);

        newCharValues.Type = CharacterValues.type.NewHunter;

        if(newCharacterSoldierList != null)
            newCharacterSoldierList.Add(newCharValues);

        newCharValues.id = CharacterController.CharactersLoaded.Count;

        return newCharValues;
    }

    CharacterValues GenerateNewCharacterStats(CharacterValues charValues)
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        for (int i = 0; i < newCharPoints; i++)
        {
            if (randomValue < damagePointsChance)
            {
                charValues.damage += 1;
            }
            else
            {
                charValues.health += 1;
            }
            randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        }

        //charValues.damageSpeed = 5; 
        //charValues.range = 5;    

        return charValues;

    }

    public GameObject GenerateCharacterFromValues(CharacterValues charValues, Vector3 position, Quaternion rotation = new Quaternion())
    {
        GameObject character = GameObject.Instantiate(Resources.Load(StringResources.charactersPrefabsPath + charValues.prefabName), position, rotation) as GameObject;

        if (charValues.materialName != null)
        {
            character.GetComponentInChildren<SkinnedMeshRenderer>().material = GameObject.Instantiate(Resources.Load(StringResources.charactersMaterialsPath + charValues.materialName) as Material);
        }
        /**/
        character.GetComponent<Character>().init(charValues);
         
        //spawn weapons 
        if (charValues.id != 0)
        {
            IEnumerable<GameObject> equips = GenerateEquippableItemsFromValues(GetCharacterEquippedItemsValues(charValues.id));
            //equip weapons
            equipItemsToCharacter(equips, character.GetComponent<Character>());
        }
        /**/

        return character;
    }
}