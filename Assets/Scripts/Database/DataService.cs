﻿using System;
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
        Debug.Log("Final PATH: " + dbPath);

    }

    public void CreateDB()
    {

        _connection.DropTable<CharacterValues>();
        _connection.DropTable<EquippableitemValues>();

        _connection.CreateTable<CharacterValues>();
        _connection.CreateTable<EquippableitemValues>();
        
        _connection.InsertAll(new[]
        {
            new CharacterValues
            {
                name = "Daniel",
                Type = CharacterValues.type.Player,
                damage = 5,
                health = 100,
                damageSpeed = 5,
                range = 5,
                prefabName = "Player"
            },
         new CharacterValues
            {
                name = "John",
                Type = CharacterValues.type.Hunter,
                damage = 8,
                health = 50,
                damageSpeed = 4,
                range = 2,
                prefabName = "Follower1"
            },
         new CharacterValues
            {
                name = "Nicolai",
                Type = CharacterValues.type.Hunter,
                damage = 5,
                health = 50,
                damageSpeed = 4,
                range = 7,
                prefabName = "Follower2"
            },
         new CharacterValues
            {
                name = "Peter",
                Type = CharacterValues.type.Hunter,
                damage = 9,
                health = 50,
                damageSpeed = 4,
                range = 2,
                prefabName = "Follower3"
            },
         new CharacterValues
            {
                name = "Christian",
                Type = CharacterValues.type.Hunter,
                damage = 3,
                health = 50,
                damageSpeed = 9,
                range = 3,
                prefabName = "Follower4"
            },
          new CharacterValues
            {
                name = "Yasmin",
                Type = CharacterValues.type.Wolf,
                tier = 6,
                damage = 6,
                health = 50,
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
          },
          new CharacterValues
          {
              name = "Young tribesman",
              Type = CharacterValues.type.Tribesman,
              tier = 1,
              damage = 2,
              health = 3,
              damageSpeed = 2,
              range = 2,
              prefabName = "EnemyMinion"
          },
          new CharacterValues
          {
              name = "Teen tribesman",
              Type = CharacterValues.type.Tribesman,
              tier = 2,
              damage = 3,
              health = 4,
              damageSpeed = 2,
              range = 2,
              prefabName = "EnemyMinion"
          },
          new CharacterValues
          {
              name = "Tribesman initiate",
              Type = CharacterValues.type.Tribesman,
              tier = 3,
              damage = 4,
              health = 5,
              damageSpeed = 2,
              range = 2,
              prefabName = "EnemyMinion"
          },
          new CharacterValues
          {
              name = "Mature tribesman",
              Type = CharacterValues.type.Tribesman,
              tier = 4,
              damage = 5,
              health = 6,
              damageSpeed = 3,
              range = 2,
              prefabName = "EnemyMinion"
          },
          new CharacterValues
          {
              name = "Leader tribesman",
              Type = CharacterValues.type.Tribesman,
              tier = 5,
              damage = 6,
              health = 7,
              damageSpeed = 3,
              range = 2,
              prefabName = "EnemyMinion"
          }

        });

        _connection.InsertAll(new[]
        {
             new EquippableitemValues
         {
             id = 1,
             name = "Stick",
             type = "Polearm",
             Slot = EquippableitemValues.slot.rightHand,
             characterId = 1,
             prefabName = "Stick"
         },
             new EquippableitemValues
         {
             id = 2,
             name = "Plastic Shield",
             type = "Shield",
             Slot = EquippableitemValues.slot.leftHand,
             prefabName = "Shield"

         }
        });

    }

    public IEnumerable<CharacterValues> GetFellowshipValues()
    {
        return _connection.Table<CharacterValues>();
    }

    //TODO generalize this function with any number of fellowship and with rotation
    public IEnumerable<GameObject> GetPlayerFellowshipInPosition(Vector3 position, Quaternion rotation = new Quaternion())
    {
        List<GameObject> fellowship = new List<GameObject>();
        //istantiate player
        GameObject daniel = GenerateCharacterByName("Daniel", position);
        //istantiate fellows and parent them to player
        GameObject john = GenerateCharacterByName("John", daniel.transform.position + Vector3.left);
        GameObject nicolai = GenerateCharacterByName("Nicolai", daniel.transform.position + Vector3.right);
        GameObject peter = GenerateCharacterByName("Peter", daniel.transform.position + Vector3.down);

        return fellowship;

    }
   
    

    public CharacterValues GetCharacterValuesByName(string characterName)
    {
        return _connection.Table<CharacterValues>().Where(x => x.name == characterName).FirstOrDefault();
        
    }
    /**/
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
                if(item != null)
                    item.GetComponent<EquippableItem>().init(itemValues);
                else
                {
                    print(itemValues.prefabName + " can not retrieve the referred prefab!");
                    return null;
                } 
                //add it to the list
                equips.Add(item);
            }else print("Prefab of equip item not found!");
        }

        return equips;
    }


    public GameObject GenerateCharacterByName(string characterName, Vector3 position, Quaternion rotation = new Quaternion())
    {
        //get informations from database
        CharacterValues charValues = GetCharacterValuesByName(characterName);
        //load character prefab, weapons prefab and attach them
        //print(StringResources.characterPrefabsPath + charValues.prefabName);
        //load prefab
        GameObject characterGameObject = Instantiate(Resources.Load(StringResources.characterPrefabsPath + charValues.prefabName), position, rotation) as GameObject;
        //assign values to prefab
        characterGameObject.GetComponent<Character>().init(charValues);
        //spawn weapons TODO handle the weapons stats
        List<GameObject> equips = GetCharacterEquippableItems(charValues.id) as List<GameObject>;
        /*
        foreach (GameObject equip in equips) //TODO handle weapons insertion!
        {
            
            Instantiate(Resources.Load(StringResources.equippableItemsPrefabsPath + equip.GetComponent<EquippableItem>().itemValues.prefabName), 
                position, Quaternion.identity);
        }
        //attach them to the player
        */
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
            charact = GetCharacterFromValues(charValues);

            characters.Add(charact);
        }

        return characters;
    }

    public GameObject GetCharacterFromValues(CharacterValues charValues)
    {
        GameObject character = Resources.Load(StringResources.characterPrefabsPath + charValues.prefabName) as GameObject;//Instantiate()  as GameObject;
                                                                                                                          /*#if UNITY_EDITOR
                                                                                                                                  PrefabUtility.DisconnectPrefabInstance(gameObject);
                                                                                                                          #endif*/
        character.GetComponent<Character>().init(charValues);
        //todo handle weapons attached to them!

        return character;
    }


    /*public IEnumerable<EquippableitemValues> GetCharacterEquippedItemsValues(string characterName)
    {
        string q = "select equip.* from  EquippableitemValues equip inner join CharacterValues " +
                   "character on equip.id = character.rightHandEquipId where character.name = 'Daniel'";
        List<EquippableitemValues> equipIds = _connection.Query<EquippableitemValues>(q);

        return equipIds;
    }*/


}
