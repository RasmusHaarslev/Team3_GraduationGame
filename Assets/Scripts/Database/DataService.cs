using System;
using System.Collections;
using SQLite;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
//using Boo.Lang;
//using UnityEditor.VersionControl;
using Enumerable = System.Linq.Enumerable;
using System.Linq;
using System.Runtime.InteropServices;


public class DataService : MonoBehaviour
{

    private SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //Debug.Log("Final PATH: " + dbPath);

    }

    public void CreateDB(int command = 0)
    {/*
		if (command == 0)*/
        if (_connection.GetTableInfo("EquippableitemValues").Any(colInfo => colInfo.Name == "level"))
        {
            print("New Database already present, continuing with the old one.");  //Databese already present, continuing with the old one. 
            return;
        }


        //Otherwise create database... print("Creating database...");
        print("Creating new version of database..");
        _connection.DropTable<CharacterValues>();
        _connection.DropTable<EquippableitemValues>();
        _connection.DropTable<InventoryItem>();

        _connection.CreateTable<CharacterValues>();
        _connection.CreateTable<EquippableitemValues>();
        _connection.CreateTable<InventoryItem>();

        //GENERATE RANDOM LEADER
        CharacterGenerator charGenerator = new CharacterGenerator();
        CharacterValues leaderValues = charGenerator.GenerateNewHunterValues(null,125, 0.07f); //pass attributes points as parameter
        leaderValues.id = 1;
        leaderValues.Type = CharacterValues.type.Player;
        leaderValues.prefabName = StringResources.playerPrefabName;
        leaderValues.materialName = StringResources.playerMaterialName;
        WeaponGenerator weaponGen = new WeaponGenerator();
        EquippableitemValues leaderWeapon = weaponGen.GenerateEquippableItem(EquippableitemValues.type.polearm, 1); //leader will have a random level 1 spear
        leaderWeapon.characterId = 1;
        //ENDING OF RANDOM LEADER GENERATION



        _connection.InsertAll(new[]
        {
            leaderValues,
            /*new CharacterValues
            {
                id = 1,
                name = "Daniel",
                isMale = true,
                Type = CharacterValues.type.Player,
                damage = 16,
                health = 115,
                damageSpeed = 1.5f,
                range = 2,
                prefabName = StringResources.playerPrefabName,
                materialName = StringResources.playerMaterialName
            },
         new CharacterValues
            {
                id = 2,
                name = "John",
                isMale = true,
                Type = CharacterValues.type.Hunter,
                damage = 8,
                health = 75,
                damageSpeed = 1f,
                range = 2,
                combatTrait = CharacterValues.CombatTrait.BraveFool,
                targetTrait = CharacterValues.TargetTrait.NoTrait,
                prefabName = StringResources.follower1PrefabName,
                materialName = StringResources.follower1MaterialName
            },
         new CharacterValues
            {
                id = 3,
                name = "Nicolai",
                isMale = true,
                Type = CharacterValues.type.Hunter,
                damage = 8,
                health = 75,
                damageSpeed = 1f,
                range = 2,
                combatTrait = CharacterValues.CombatTrait.Clingy,
                targetTrait = CharacterValues.TargetTrait.Loyal,
                prefabName = StringResources.follower1PrefabName,
                materialName = StringResources.follower1MaterialName
            },
         new CharacterValues
            {
                id = 3,
                name = "Peter",
                isMale = true,
                Type = CharacterValues.type.Hunter,
                damage = 8,
                health = 75,
                damageSpeed = 1f,
                range = 2,
                combatTrait = CharacterValues.CombatTrait.Fearful,
                targetTrait = CharacterValues.TargetTrait.LowAttentionSpan,
                prefabName = StringResources.follower1PrefabName,
                materialName = StringResources.follower1MaterialName
            },
          new CharacterValues
            {
                name = "Yasmin",
                isMale = false,
                Type = CharacterValues.type.Wolf,
                tier = 6,
                damage = 8,
                health = 75,
                damageSpeed = 1,
                range = 2,
                prefabName = "EnemyLeader"
            },

          new CharacterValues
          {
              name = "Young wolf",
              Type = CharacterValues.type.Wolf,
              tier = 1,
              damage = 2,
              health = 3,
              damageSpeed = 2,
              range = 2,
              prefabName = "Wolf"
          },
          new CharacterValues
          {
              name = "Teen wolf",
              Type = CharacterValues.type.Wolf,
              tier = 2,
              damage = 3,
              health = 4,
              damageSpeed = 2,
              range = 2,
              prefabName = "Wolf"
          },
          new CharacterValues
          {
              name = "Wolf initiate",
              Type = CharacterValues.type.Wolf,
              tier = 3,
              damage = 4,
              health = 5,
              damageSpeed = 2,
              range = 2,
              prefabName = "Wolf"
          },
          new CharacterValues
          {
              name = "Mature wolf",
              Type = CharacterValues.type.Wolf,
              tier = 4,
              damage = 5,
              health = 6,
              damageSpeed = 2,
              range = 2,
              prefabName = "Wolf"
          },
          new CharacterValues
          {
              name = "Leader wolf",
              Type = CharacterValues.type.Wolf,
              tier = 5,
              damage = 6,
              health = 7,
              damageSpeed = 2,
              range = 2,
              prefabName = "Wolf"
          },*/
          new CharacterValues
          {
              id = 4,
              name = "Easy mellee tribesman",
              Type = CharacterValues.type.Tribesman,
              tier = 1,
              damage = 3,
              health = 35,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier1-2Material"
          },
          new CharacterValues
          {
              id = 5,
              name = "Easy rifle tribesman",
              Type = CharacterValues.type.Tribesman,
              tier = 2,
              damage = 3,
              health = 35,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier1-2Material"
          },
          new CharacterValues
          {
              id = 6,
              name = "Medium mellee tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 3,
              damage = 6,
              health = 35,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier3-4Material"
          },
          new CharacterValues
          {
              id = 7,
              name = "Medium rifle tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 4,
              damage = 6,
              health = 35,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier3-4Material"
          },
            new CharacterValues
          {
                id = 8,
              name = "Hard mellee tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 5,
              damage = 6,
              health = 35,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier5-6Material"
          },
            new CharacterValues
          {
                id = 9,
              name = "Hard rifle tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 6,
              damage = 6,
              health = 35,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier5-6Material"
          },
        });

        _connection.InsertAll(new[]
        { //WEAPONS
            leaderWeapon,
            /*
             new EquippableitemValues
         {
             id = 1,
             name = "Toothpick",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 20,
             damage = 10,
             damageSpeed = 2f,
             range = 2,
             characterId = 1,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },
             new EquippableitemValues
         {
             id = 2,
             name = "Plastic Shield",
             Type = EquippableitemValues.type.shield,
             Slot = EquippableitemValues.slot.leftHand,
             health = 20,
             damage = 10,
             damageSpeed = 2f,
             range = 2,
             characterId = 2,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.shield][0][1]
         },
             new EquippableitemValues
         {
             id = 3,
             name = "Laser Rifle 2000",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             health = 20,
             damage = 10,
             damageSpeed = 2.5f,
             range = 20,
             characterId = 3,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][0][1]
         },
             new EquippableitemValues
         {
             id = 4,
             name = "Rifle of the Git Master Rasmus",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             health = 25,
             damage = 15,
             damageSpeed = 1.5f,
             range = 15,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][0][1]
         },
             new EquippableitemValues
         {
             id = 5,
             name = "Mighty power Stick",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 20,
             damage = 20,
             damageSpeed = 1.5f,
             range = 2,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },
             new EquippableitemValues
         {
             id = 6,
             name = "Romanian Steel Bar",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 20,
             damage = 25,
             damageSpeed = 1.5f,
             range = 2,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },*/
             new EquippableitemValues
         {
             //id = 6,
             name = "Easy Steel Bar",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 2,
             characterId = 4,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },new EquippableitemValues
         {
             //id = 4,
             name = "Easy Rifle",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 9,
             characterId = 5,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][0][1]
         },
             new EquippableitemValues
         {
             //id = 6,
             name = "Medium Steel Bar",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 2,
             characterId = 6,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },new EquippableitemValues
         {
             //id = 4,
             name = "Medium Rifle",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 9,
             characterId = 7,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][0][1]
         },
             new EquippableitemValues
         {
             //id = 6,
             name = "Hard Steel Bar",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 2,
             characterId = 8,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },new EquippableitemValues
         {
             //id = 4,
             name = "Hard Rifle",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 9,
             characterId = 9,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][0][1]
         }
        });
        /* INVENTORY ITEMS
        _connection.InsertAll(new[]
        {
            
            new InventoryItem
            {
                Type = InventoryItem.type.equippable,
                deferredId = 4,
                quantity = 1
            },
            new InventoryItem
            {
                Type = InventoryItem.type.equippable,
                deferredId = 5,
                quantity = 1
            },
            new InventoryItem
            {
                Type = InventoryItem.type.equippable,
                deferredId = 6,
                quantity = 1
            }
            

        });*/

    }

    public void ResetDatabase()
    {
        _connection.DropTable<CharacterValues>();
        _connection.DropTable<EquippableitemValues>();
        _connection.DropTable<InventoryItem>();

        CreateDB();
    }

    #region character methods
    

    public CharacterValues[] GetNewHuntersValues()
    {
        return (_connection.Table<CharacterValues>().Where(x => x.Type == CharacterValues.type.NewHunter)).ToArray();
    }

    /// <summary>
    /// Add values to db and returns the id
    /// </summary>
    /// <param name="hunterValues"></param>
    /// <returns></returns>
    /// 
    public void UpdateCharacterValuesInDb(CharacterValues charValToUpdate)
    {
        _connection.Update(charValToUpdate);
    }
    public void DeleteCharactersValuesFromDb(CharacterValues charValuesToDelete)
    {
        
            _connection.Delete(charValuesToDelete); 
        //delete all the equipped items associated to that character
        _connection.Query<InventoryItem>("DELETE FROM EquippableitemValues WHERE characterId = " + charValuesToDelete.id );

    }

    public int AddcharacterToDbByValues(CharacterValues hunterValues)
    {
        _connection.Insert(hunterValues);
        return _connection.ExecuteScalar<int>("SELECT last_insert_rowid()"); 
    }

    public IEnumerable<CharacterValues> GetFellowshipValues()
    {
        return _connection.Table<CharacterValues>().Where(x => x.Type == CharacterValues.type.Hunter || x.Type == CharacterValues.type.Player);

    }

    //TODO generalize this function with any number of fellowship and with rotation
    public GameObject GetPlayerFellowshipInPosition(Transform fellowshipLocation)
    {
        CharacterSpawner[] spawners = fellowshipLocation.gameObject.GetComponentsInChildren<CharacterSpawner>().ToArray();
        GameObject fellowship = new GameObject("PlayerFellowship");
        CharacterValues[] fellowshipValues = GetFellowshipValues().ToArray();

        if (spawners.Length == 4)
        {
            for(int i=0; i < fellowshipValues.Length;i++)
            {
                //istantiate player
                GameObject charGameObject = GenerateCharacterFromValues(fellowshipValues[i], spawners[i].transform.position, spawners[i].transform.rotation);
                charGameObject.transform.parent = fellowship.transform;
            }            
        }
        else
        {
            print("Missing some character spawners for the player and/or fellows!");
        }


        return fellowship;

    }

    public bool RemoveCharacter(CharacterValues charToRemove)
    {
        if(charToRemove.id != 0)
        {
            _connection.Delete(charToRemove);

            return true;
        }
        return false;
    }

    public CharacterValues GetCharacterValuesByName(string characterName)
    {
        return _connection.Table<CharacterValues>().Where(x => x.name == characterName).FirstOrDefault();

    }

    public IEnumerable<EquippableitemValues> GetCharacterEquippableItemsValues(int characterId)
    {
        string q = "select equip.* from  EquippableitemValues equip where equip.characterId = ?";
        List<EquippableitemValues> equipIds = _connection.Query<EquippableitemValues>(q, characterId);

        return equipIds;
    }

    /// <summary>
    /// Gives the gameobject list of equippable items for the given character id.
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public IEnumerable<GameObject> GetCharacterEquippableItems(int characterId)
    {
        List<EquippableitemValues> equipsValues = GetCharacterEquippableItemsValues(characterId).ToList();
        List<GameObject> equips = new List<GameObject>();
        foreach (EquippableitemValues itemValues in equipsValues)
        {
            if (itemValues.prefabName != null)
            {
                GameObject item = Resources.Load(StringResources.equippableItemsPrefabsPath + itemValues.prefabName) as GameObject;

                //put values into the prefab
                if (item != null)
                    item.GetComponent<EquippableItem>().init(itemValues);
                else
                {
                    print(itemValues.prefabName + " can not retrieve the referred prefab!");
                    return null;
                }
                //add it to the list
                equips.Add(item);
            }
            else print("Prefab of equip item not found!");
        }

        return equips;
    }


    public GameObject GenerateCharacterByName(string characterName, Vector3 position, Quaternion rotation = new Quaternion())
    {
        //get informations from database
        CharacterValues charValues = GetCharacterValuesByName(characterName);
        GameObject characterGameObject = GenerateCharacterFromValues(charValues, position, rotation);

        return characterGameObject;
    }

    public IEnumerable<CharacterValues> GetCharactersValuesByType(CharacterValues.type charactersType)
    {
        string q = "select * from  CharacterValues where Type = ?";
        return _connection.Query<CharacterValues>(q, charactersType);
    }

    public IEnumerable<GameObject> GenerateCharactersByType(CharacterValues.type charactersType)
    {
        List<GameObject> characters = new List<GameObject>();
        GameObject charact;
        List<CharacterValues> charValuesList = GetCharactersValuesByType(charactersType).ToList();
        foreach (CharacterValues charValues in charValuesList)
        {
            charact = GenerateCharacterFromValues(charValues, Vector3.zero);

            characters.Add(charact);
        }

        return characters;
    }

    public GameObject GenerateCharacterFromValues(CharacterValues charValues, Vector3 position, Quaternion rotation = new Quaternion())
    {
        GameObject character = Instantiate(Resources.Load(StringResources.charactersPrefabsPath + charValues.prefabName), position, rotation) as GameObject;

        if (charValues.materialName != null)
        {
            character.GetComponentInChildren<SkinnedMeshRenderer>().material = Instantiate(Resources.Load(StringResources.charactersMaterialsPath + charValues.materialName) as Material);
        }
        /**/
        character.GetComponent<Character>().init(charValues);

        //spawn weapons 
        if(charValues.id != 0) { 
        IEnumerable<GameObject> equips = GenerateEquippableItemsFromValues(GetCharacterEquippedItemsValues(charValues.id));
        //equip weapons
        equipItemsToCharacter(equips, character.GetComponent<Character>());
        }
        /**/

        return character;
    }
    #endregion
    /**/
    #region equippable items methods
    public IEnumerable<EquippableitemValues> GetCharacterEquippedItemsValues(int characterId)
    {
        string q = "select * from  EquippableitemValues where characterId = ? ";
        List<EquippableitemValues> equipsValues = _connection.Query<EquippableitemValues>(q, characterId);

        return equipsValues;
    }

    public IEnumerable<GameObject> GenerateEquippableItemsFromValues(IEnumerable<EquippableitemValues> equipValues)
    {
        List<GameObject> equips = new List<GameObject>();
        GameObject currentEquip;

        foreach (EquippableitemValues values in equipValues)
        {
            currentEquip = Instantiate(Resources.Load(StringResources.equippableItemsPrefabsPath + values.prefabName)) as GameObject;
            
            if (values.materialName != null)
            {
                currentEquip.GetComponent<Renderer>().material = Instantiate(Resources.Load(StringResources.itemsMaterialsPath + values.materialName) as Material);
            }
            currentEquip.GetComponent<EquippableItem>().init(values);

            equips.Add(currentEquip);
        }

        return equips;
    }

    /// <summary>
    /// Changes the stats and spawn the item on the right character slot
    /// </summary>
    /// <param name="item"></param>
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
                
                //into the database
                if (currentEquipValues.id != 0) {
                currentEquipValues.characterId = character.characterBaseValues.id;
                _connection.Update(currentEquipValues);
                //remove from inventory
                _connection.Query<InventoryItem>("DELETE FROM InventoryItem WHERE Type = " + (int)InventoryItem.type.equippable + " and deferredId = " + currentEquipValues.id);
                }
            }
            else
            {
                print("Trying to equip " + equip.name + " that is not an equippable item!");

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
            _connection.Update(itemToDetatch.itemValues);
            //add it in inventory
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Type = InventoryItem.type.equippable;
            inventoryItem.deferredId = itemToDetatch.itemValues.id;
            _connection.Insert(inventoryItem);
            //removing from scene
            Destroy(itemToDetatch.transform.gameObject);
        }
    }


    public GameObject GenerateNewEquippableItemFromValues(EquippableitemValues values)
    {
        _connection.Insert(values);
        values.id = _connection.ExecuteScalar<int>("SELECT last_insert_rowid()");

        return GenerateEquippableItemsFromValues(new List<EquippableitemValues>() { values }).FirstOrDefault();        
    }

    public int AddWeaponInDbByValues(EquippableitemValues itemValues)
    {
        _connection.Insert(  itemValues);

        return _connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
    }

    public IEnumerable<EquippableitemValues> GetEquippableItemsValuesFromInventory()
    {
        List<EquippableitemValues> itemsValues = new List<EquippableitemValues>();

        string query =
            "SELECT itemValues.* FROM EquippableitemValues AS itemValues JOIN InventoryItem AS inventory ON itemValues.id = inventory.deferredId WHERE inventory.Type = " + (int)InventoryItem.type.equippable;

        itemsValues = _connection.Query<EquippableitemValues>(query);

        return itemsValues;
    }

    public void InsertItemsValuesInInventory(IEnumerable<EquippableitemValues> itemsValues )
    {
        foreach (EquippableitemValues itemValues in itemsValues)
        {
            //from database
            _connection.Insert(itemValues);
            //add it in inventory
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Type = InventoryItem.type.equippable;
            inventoryItem.deferredId = itemValues.id;
            _connection.Insert(inventoryItem);
        }
       
    }
    #endregion

}
