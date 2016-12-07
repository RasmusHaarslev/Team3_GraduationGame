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

    private SQLiteAsyncConnection _asyncConnection;

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
        _connection.BusyTimeout = TimeSpan.FromSeconds(2);
        _asyncConnection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
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
        CharacterValues leaderValues = charGenerator.GenerateNewHunterValues(null, 145, 0.15f); //pass attributes points as parameter
        leaderValues.id = 1;
        leaderValues.Type = CharacterValues.type.Player;
        leaderValues.isMale = true; //Leader can only be male!
        leaderValues.name = StringResources.maleNames[UnityEngine.Random.Range(0, StringResources.maleNames.Length)]; //male name
        leaderValues.prefabName = StringResources.playerPrefabName;
        leaderValues.materialName = StringResources.playerMaterialName;
        WeaponGenerator weaponGen = new WeaponGenerator();
        EquippableitemValues leaderWeapon = weaponGen.GenerateEquippableItem(EquippableitemValues.type.polearm, 1, 0.2f, 0.7f, 0.1f); //leader will have a random level 1 spear
        leaderWeapon.characterId = 1;                                                                                   //damage, health and dmg-speed probability
        //ENDING OF RANDOM LEADER GENERATION



        _connection.InsertAll(new[]
        {
            leaderValues,
          new CharacterValues
            {
                id = 2,
                name = "Aleksy",
                isMale = true,
                Type = CharacterValues.type.Hunter,
                damage = 4,
                health = 96,
                damageSpeed = 1f,
                range = 2,
                combatTrait = CharacterValues.CombatTrait.BraveFool,
                targetTrait = CharacterValues.TargetTrait.NoTrait,
                prefabName = StringResources.follower1PrefabName,
                materialName = StringResources.maleHuntersMaterials[1]
            },
         new CharacterValues
            {
                id = 3,
                name = "Yazmin",
                isMale = false,
                Type = CharacterValues.type.Hunter,
                damage = 6,
                health = 94,
                damageSpeed = 1f,
                range = 2,
                combatTrait = CharacterValues.CombatTrait.Clingy,
                targetTrait = CharacterValues.TargetTrait.Loyal,
                prefabName = StringResources.follower1PrefabName,
                materialName = StringResources.femaleHuntersMaterials[4]
            },
         new CharacterValues
            {
                id = 4,
                name = "Zeheb",
                isMale = true,
                Type = CharacterValues.type.Hunter,
                damage = 8,
                health = 92,
                damageSpeed = 1f,
                range = 2,
                combatTrait = CharacterValues.CombatTrait.Fearful,
                targetTrait = CharacterValues.TargetTrait.LowAttentionSpan,
                prefabName = StringResources.follower1PrefabName,
                materialName = StringResources.maleHuntersMaterials[8]
            },
          new CharacterValues
          {
              id = 5,
              name = "Easy melee tribesman",
              Type = CharacterValues.type.Tribesman,
              isMale = true,
              tier = 1,
              damage = 3,
              health = 65,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier1-2Material"
          },
          new CharacterValues
          {
              id = 6,
              name = "Easy rifle tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 2,
              damage = 3,
              health = 65,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier1-2Material"
          },
          new CharacterValues
          {
              id = 7,
              name = "Medium melee tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 3,
              damage = 6,
              health = 75,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier3-4Material"
          },
          new CharacterValues
          {
              id = 8,
              name = "Medium rifle tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 4,
              damage = 6,
              health = 75,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier3-4Material"
          },
            new CharacterValues
          {
                id = 9,
              name = "Hard melee tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 5,
              damage = 10,
              health = 85,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier5-6Material"
          },
            new CharacterValues
          {
                id = 10,
              name = "Hard rifle tribesman",
              isMale = true,
              Type = CharacterValues.type.Tribesman,
              tier = 6,
              damage = 10,
              health = 85,
              damageSpeed = 2,
              range = 2,
              prefabName = "Rival",
              materialName = "RivalTribesmanTier5-6Material"
          },
        });

        _connection.InsertAll(new[]
        { //WEAPONS

            leaderWeapon,
             new EquippableitemValues
         {
             id = 2,//john weapon
             name = "Initiate Rifle",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             level = 1,
             health = 4,
             damage = 6,
             damageSpeed = 2f,
             range = 9,
             characterId = 2,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][0][1]
         },
             new EquippableitemValues
         {
             id = 3, //Nicolai weapom
             name = "Initiate Spear",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             level = 1,
             health = 5,
             damage = 5,
             damageSpeed = 2.5f,
             range = 2,
             characterId = 3,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },
             new EquippableitemValues
         {
             id = 4, //Peter weapon
             name = "Initiate Shield",
             Type = EquippableitemValues.type.shield,
             Slot = EquippableitemValues.slot.leftHand,
             level = 1,
             health = 8,
             damage = 2,
             damageSpeed = 1.5f,
             range = 2,
             characterId = 4,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.shield][0][1]
         },
             new EquippableitemValues
         {
             id = 5,
             name = "Easy Steel Bar",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 2,
             characterId = 5,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },new EquippableitemValues
         {
             id = 6,
             name = "Easy Rifle",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 9,
             characterId = 6,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][0][1]
         },
             new EquippableitemValues
         {
             id = 7,
             name = "Medium Steel Bar",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 2,
             characterId = 7,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },new EquippableitemValues
         {
             id = 8,
             name = "Medium Rifle",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 9,
             characterId = 8,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.rifle][0][1]
         },
             new EquippableitemValues
         {
             id = 9,
             name = "Hard Steel Bar",
             Type = EquippableitemValues.type.polearm,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 2,
             characterId = 9,
             prefabName = StringResources.equipItemsModelsStrings[EquippableitemValues.type.polearm][0][1]
         },new EquippableitemValues
         {
             id = 10,
             name = "Hard Rifle",
             Type = EquippableitemValues.type.rifle,
             Slot = EquippableitemValues.slot.rightHand,
             health = 0,
             damage = 0,
             damageSpeed = 2f,
             range = 9,
             characterId = 10,
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

    public GameObject spawnCharacterGameObject(CharacterValues charValues, EquippableitemValues equipValues, Transform position = null)
    {
        GameObject character;
        if (position != null)
        {
            character = Instantiate(Resources.Load(StringResources.charactersPrefabsPath + charValues.prefabName), position) as GameObject;
        }

        else
        {
            character = Instantiate(Resources.Load(StringResources.charactersPrefabsPath + charValues.prefabName)) as GameObject;
        }


        if (charValues.materialName != null)
        {
            character.GetComponentInChildren<SkinnedMeshRenderer>().material = Instantiate(Resources.Load(StringResources.charactersMaterialsPath + charValues.materialName) as Material);
        }
        /**/
        character.GetComponent<Character>().init(charValues);
        character.transform.localPosition = Vector3.zero;
        //spawn weapon
        GameObject currentEquip = Instantiate(Resources.Load(StringResources.equippableItemsPrefabsPath + equipValues.prefabName)) as GameObject;

        if (equipValues.materialName != null)
        {
            currentEquip.GetComponent<Renderer>().material = Instantiate(Resources.Load(StringResources.itemsMaterialsPath + equipValues.materialName) as Material);
        }
        currentEquip.GetComponent<EquippableItem>().init(equipValues);

        //equip weapon
        equipItemToCharacterGameObject(ref currentEquip, ref character);


        return character;
    }

    public async void UpdateCharactersValues(CharacterValues[] charsValues)
    {

        foreach (CharacterValues val in charsValues)
            await _asyncConnection.InsertOrReplaceAsync(val);
    }

    public CharacterValues[] GetNewHuntersValues()
    {
        return _connection.Query<CharacterValues>("SELECT * FROM CharacterValues WHERE id IN (20, 21, 22)").ToArray();
    }
    public EquippableitemValues[] GetNewHuntersEquipValues()
    {
        return _connection.Query<EquippableitemValues>("SELECT * FROM EquippableitemValues WHERE id IN (20, 21, 22)").ToArray();
    }
    /// <summary>
    /// Add values to db and returns the id
    /// </summary>
    /// <param name="hunterValues"></param>
    /// <returns></returns>
    /// 
    public async void UpdateCharacterValuesInDb(CharacterValues charValToUpdate)
    {
        await _asyncConnection.InsertOrReplaceAsync(charValToUpdate);
    }
    public async void DeleteCharactersValuesFromDb(CharacterValues charValuesToDelete)
    {

        await _asyncConnection.DeleteAsync(charValuesToDelete);
        //delete all the equipped items associated to that character
        await _asyncConnection.QueryAsync<InventoryItem>("DELETE FROM EquippableitemValues WHERE characterId = " + charValuesToDelete.id);

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
        CharacterValues[] fellowshipValues = GetFellowshipValues().ToArray(); //order by ascending ID. The first one is Always the player.
 
        if (spawners.Length > 0)
        {
            CharacterValues currentvalues;
            //Spawn characters based on how many spawners I find
            for (int i = 0; i < spawners.Length; i++)
            {
                currentvalues = fellowshipValues.FirstOrDefault(x => x.id == spawners[i].tier);
                if(currentvalues != null) { 
                //istantiate a character with the id specified in the Tier of the character spawner
                GameObject charGameObject = GenerateCharacterFromValues(currentvalues, spawners[i].transform.position, spawners[i].transform.rotation);
                charGameObject.transform.parent = fellowship.transform;
                }
            }
        }
        else
        {
            print("No spawners found for the player and/or fellows!");
        }

        /*
        if (spawners.Length == 4)
        {
            for (int i = 0; i < fellowshipValues.Length; i++)
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
        */

        return fellowship;

    }

    public bool RemoveCharacter(CharacterValues charToRemove)
    {
        if (charToRemove.id != 0)
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
        if (charValues.id != 0)
        {
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

    public IEnumerable<EquippableitemValues> GetFellowshipEquippableItemsValues()
    { 
        return _connection.Query<EquippableitemValues>("SELECT * FROM EquippableitemValues WHERE characterId IN (1, 2, 3, 4)");
    }
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

    public void equipItemToCharacterGameObject(ref GameObject equip, ref GameObject charGO)
    {
        EquippableitemValues currentEquipValues;
        Character character = charGO.GetComponent<Character>();
        currentEquipValues = equip.GetComponent<EquippableItem>().itemValues;
        if (currentEquipValues != null)
        {
            //handling the "contstraint" to have only one weapon equipped, and considering the shield as a weapon, if the shield is present on the left, it will be removed
            if (currentEquipValues.Type == EquippableitemValues.type.polearm ||
                currentEquipValues.Type == EquippableitemValues.type.rifle ||
                currentEquipValues.Type == EquippableitemValues.type.shield)
            {
                detatchItemFromCharacterGameObject(EquippableitemValues.slot.leftHand, ref character);
                detatchItemFromCharacterGameObject(EquippableitemValues.slot.rightHand, ref character);
            }
            //checking if another item is equipped in the item slot
            else if (character.equippableSpots[currentEquipValues.Slot].GetComponentInChildren<EquippableItem>() != null)
            {
                //if thats the case, remove the values and remove the old object
                detatchItemFromCharacterGameObject(currentEquipValues.Slot, ref character);
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
            /*
            if (currentEquipValues.id != 0)
            {
                currentEquipValues.characterId = character.characterBaseValues.id;
                _connection.Update(currentEquipValues);
                //remove from inventory
                _connection.Query<InventoryItem>("DELETE FROM InventoryItem WHERE Type = " + (int)InventoryItem.type.equippable + " and deferredId = " + currentEquipValues.id);
            }
            */
        }
        else
        {
            print("Trying to equip " + equip.name + " that is not an equippable item!");

        }


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
                if (currentEquipValues.id != 0)
                {
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

    public void detatchItemFromCharacterGameObject(EquippableitemValues.slot slotToDetatch, ref Character character)
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
            /*
            //putting that from character into inventory
            itemToDetatch.itemValues.characterId = 0;
            _connection.Update(itemToDetatch.itemValues);
            //add it in inventory
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Type = InventoryItem.type.equippable;
            inventoryItem.deferredId = itemToDetatch.itemValues.id;
            _connection.Insert(inventoryItem);
            */
            //removing from scene
            Destroy(itemToDetatch.transform.gameObject);
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

    public async void UpdateEquipItemValues(EquippableitemValues itemValues)
    {
        
        await _asyncConnection.InsertOrReplaceAsync(itemValues);
    }
    public async void UpdateEquipItemsValues(EquippableitemValues[] itemsValues)
    {
        foreach (EquippableitemValues val in itemsValues)
            await _asyncConnection.InsertOrReplaceAsync(val);
    }
    public GameObject GenerateNewEquippableItemFromValues(EquippableitemValues values)
    {
        _connection.Insert(values);
        values.id = _connection.ExecuteScalar<int>("SELECT last_insert_rowid()");

        return GenerateEquippableItemsFromValues(new List<EquippableitemValues>() { values }).FirstOrDefault();
    }

    public int AddWeaponInDbByValues(EquippableitemValues itemValues)
    {
        _connection.Insert(itemValues);

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

    public void InsertItemsValuesInInventory(IEnumerable<EquippableitemValues> itemsValues)
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
