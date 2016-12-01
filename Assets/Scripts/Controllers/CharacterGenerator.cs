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

    public IEnumerable<EquippableitemValues> GetCharacterEquippedItemsValues(int characterId)
    {
        string q = "select * from  EquippableitemValues where characterId = ? ";
        List<EquippableitemValues> equipsValues = ItemController.ItemsLoaded.Where(c => c.characterId == characterId).ToList();

        return equipsValues;
    }

    public IEnumerable<GameObject> GenerateEquippableItemsFromValues(IEnumerable<EquippableitemValues> equipValues)
    {
        List<GameObject> equips = new List<GameObject>();
        GameObject currentEquip;

        foreach (EquippableitemValues values in equipValues)
        {
            currentEquip = GameObject.Instantiate(Resources.Load(StringResources.equippableItemsPrefabsPath + values.prefabName)) as GameObject;

            if (values.materialName != null)
            {
                currentEquip.GetComponent<Renderer>().material = GameObject.Instantiate(Resources.Load(StringResources.itemsMaterialsPath + values.materialName) as Material);
            }
            currentEquip.GetComponent<EquippableItem>().init(values);

            equips.Add(currentEquip);
        }

        return equips;
    }

    public void equipItemsToCharacter(IEnumerable<GameObject> equips, Character character)
    {
        EquippableitemValues currentEquipValues;
        foreach (GameObject equip in equips)
        {
            currentEquipValues = equip.GetComponent<EquippableItem>().itemValues;
            if (currentEquipValues != null)
            {
                //handling the "contstraint" to have only one weapon equipped, and considering the shield as a weapon, if the shield is present on the left, it will be removed
                if (currentEquipValues.Type == EquippableitemValues.type.polearm ||
                    currentEquipValues.Type == EquippableitemValues.type.rifle ||
                    currentEquipValues.Type == EquippableitemValues.type.shield)
                {
                    detatchItemFromCharacter(EquippableitemValues.slot.leftHand, character);
                    detatchItemFromCharacter(EquippableitemValues.slot.rightHand, character);
                }
                //checking if another item is equipped in the item slot
                else if (character.equippableSpots[currentEquipValues.Slot].GetComponentInChildren<EquippableItem>() != null)
                {
                    //if thats the case, remove the values and remove the old object
                    detatchItemFromCharacter(currentEquipValues.Slot, character);
                }

                //parent and position the item on the appropriate slot
                equip.transform.parent = character.equippableSpots[currentEquipValues.Slot];
                equip.transform.localPosition = equip.GetComponent<EquippableItem>().weaponPosition;
                equip.transform.localRotation = Quaternion.Euler(equip.GetComponent<EquippableItem>().weaponRotation);
                //handling animations
                switch (currentEquipValues.Type)
                {
                    case EquippableitemValues.type.polearm:
                        character.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(StringResources.animControllerSpearName) as RuntimeAnimatorController;
                        character.equippedWeaponType = EquippableitemValues.type.polearm;
                        break;
                    case EquippableitemValues.type.shield:
                        character.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(StringResources.animControllerShieldName) as RuntimeAnimatorController;
                        character.equippedWeaponType = EquippableitemValues.type.shield;
                        break;
                    case EquippableitemValues.type.rifle:
                        character.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(StringResources.animControllerRifleName) as RuntimeAnimatorController;
                        character.equippedWeaponType = EquippableitemValues.type.rifle;
                        break;
                }
                //add the new item values
                //to the character prefab
                //Debug.Log(character.characterBaseValues.name + " " + currentEquipValues.name);
                character.health += currentEquipValues.health;
                character.damage += currentEquipValues.damage;
                character.damageSpeed = currentEquipValues.damageSpeed;
                character.range = currentEquipValues.range;

                ////into the database
                //if (currentEquipValues.id != 0)
                //{
                //    currentEquipValues.characterId = character.characterBaseValues.id;
                //    _connection.Update(currentEquipValues);
                //    //remove from inventory
                //    _connection.Query<InventoryItem>("DELETE FROM InventoryItem WHERE Type = " + (int)InventoryItem.type.equippable + " and deferredId = " + currentEquipValues.id);
                //}
            }
            else
            {
                Debug.Log("Trying to equip " + equip.name + " that is not an equippable item!");
            }
        }

    }

    public void detatchItemFromCharacter(EquippableitemValues.slot slotToDetatch, Character character)
    {
        //remove item values from total on the player
        EquippableItem itemToDetatch = character.equippableSpots[slotToDetatch].GetComponentInChildren<EquippableItem>();
        //detatch and remove the item from the game
        if (itemToDetatch != null)
        {
            //remove the item values
            //to the character prefab
            character.health -= itemToDetatch.healthIncrease;
            character.damage -= itemToDetatch.damageIncrease;
            character.damageSpeed = character.characterBaseValues.damageSpeed;
            character.range = character.characterBaseValues.range;
            //from database
            //putting that from character into inventory
            itemToDetatch.itemValues.characterId = 0;
            //_connection.Update(itemToDetatch.itemValues);
            //add it in inventory
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Type = InventoryItem.type.equippable;
            inventoryItem.deferredId = itemToDetatch.itemValues.id;
            //_connection.Insert(inventoryItem);
            //removing from scene
            GameObject.Destroy(itemToDetatch.transform.gameObject);
        }
    }
}