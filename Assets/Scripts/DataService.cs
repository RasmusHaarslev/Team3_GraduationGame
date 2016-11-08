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
        

        _connection.CreateTable<CharacterValues>();
        _connection.CreateTable<EquippableitemValues>();

        _connection.InsertAll(new[]
        {
            new CharacterValues
            {
                name = "Daniel",
                type = "Player",
                damage = 5,
                health = 5,
                damageSpeed = 5,
                range = 5,
                prefabName = "Player"
            },
         new CharacterValues
            {
                name = "John",
                type = "FellowTribesman",
                damage = 8,
                health = 5,
                damageSpeed = 4,
                range = 2,
                prefabName = "Follower1"
            },
         new CharacterValues
            {
                name = "Nicolai",
                type = "FellowTribesman",
                damage = 5,
                health = 6,
                damageSpeed = 4,
                range = 7,
                prefabName = "Follower2"
            },
         new CharacterValues
            {
                name = "Peter",
                type = "FellowTribesman",
                damage = 9,
                health = 2,
                damageSpeed = 4,
                range = 2,
                prefabName = "Follower3"
            },
         new CharacterValues
            {
                name = "Christian",
                type = "FellowTribesman",
                damage = 3,
                health = 7,
                damageSpeed = 9,
                range = 3,
                prefabName = "Follower4"
            },
          new CharacterValues
            {
                name = "Yasmin",
                type = "AlphaMaleWolf",
                damage = 6,
                health = 1000,
                damageSpeed = 4,
                range = 2,
                prefabName = "EnemyLeader"
            }

        });

        _connection.InsertAll(new[]
        {
             new EquippableitemValues
         {
             id = 1,
             name = "Stick",
             type = "Polearm",
             slot = "rightHand",
             characterId = 1
         },
             new EquippableitemValues
         {
             id = 2,
             name = "Plastic Shield",
             type = "Shield",
             slot = "leftHand"

         }
        });

    }

    public IEnumerable<CharacterValues> GetFellowshipValues()
    {
        return _connection.Table<CharacterValues>();
    }


   
    

    public CharacterValues GetCharacterValuesByName(string characterName)
    {
        return _connection.Table<CharacterValues>().Where(x => x.name == characterName).FirstOrDefault();
        // _connection.Table<Person>().Join(_connection.Table<EquippableItem>(), x => x.Id, y => y.PersonId, (x,y) => new ArrayList {x,y});
        // _connection.Table<Person>().Select(x => x.Name == "Johnny");

    }

    public IEnumerable<EquippableitemValues> GetCharacterEquippedItemsValues(string characterName)
    {
        /**/
        string q = "select equip.* from  EquippableitemValues equip inner join CharacterValues " +
                   "character on equip.id = character.rightHandEquipId where character.name = 'Daniel'";
        List<EquippableitemValues> equipIds = _connection.Query<EquippableitemValues>(q);

        return equipIds;



    }


}
