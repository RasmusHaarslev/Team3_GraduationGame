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
	{/**/
		if (command == 0)
			if (_connection.GetTableInfo("CharacterValues").Any())
			{
				//Databese already present, continuing with the old one. print("Databese already present, continuing with the old one.");
				return;
			}
		//Otherwise create database... print("Creating database...");

		_connection.DropTable<CharacterValues>();
		_connection.DropTable<EquippableitemValues>();
		_connection.DropTable<InventoryItem>();

		_connection.CreateTable<CharacterValues>();
		_connection.CreateTable<EquippableitemValues>();
		_connection.CreateTable<InventoryItem>();

		_connection.InsertAll(new[]
		{
			new CharacterValues
			{
				id = 1,
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
				id = 2,
				name = "John",
				Type = CharacterValues.type.Hunter,
				damage = 8,
				health = 50,
				damageSpeed = 4,
				range = 2,
				combatTrait = CharacterValues.CombatTrait.BraveFool,
				targetTrait = CharacterValues.TargetTrait.Bully,
				prefabName = "Follower"
			},
		 new CharacterValues
			{
				id = 3,
				name = "Nicolai",
				Type = CharacterValues.type.Hunter,
				damage = 5,
				health = 50,
				damageSpeed = 4,
				range = 7,
				combatTrait = CharacterValues.CombatTrait.Desperate,
				targetTrait = CharacterValues.TargetTrait.GlorySeeker,
				prefabName = "Follower"
			},
		 new CharacterValues
			{
				id = 3,
				name = "Peter",
				Type = CharacterValues.type.Hunter,
				damage = 9,
				health = 50,
				damageSpeed = 4,
				range = 2,
				combatTrait = CharacterValues.CombatTrait.VeryUnlikable,
				targetTrait = CharacterValues.TargetTrait.LowAttentionSpan,
				prefabName = "Follower"
			},
		 new CharacterValues
			{
				id = 4,
				name = "Christian",
				Type = CharacterValues.type.Hunter,
				damage = 3,
				health = 50,
				damageSpeed = 9,
				range = 3,
				combatTrait = CharacterValues.CombatTrait.Vengeful,
				targetTrait = CharacterValues.TargetTrait.Codependant,
				prefabName = "Follower"
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
			  prefabName = "Rival"
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
			  prefabName = "Rival"
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
			  prefabName = "Rival"
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
			  prefabName = "Rival"
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
			  prefabName = "Rival"
		  }

		});

		_connection.InsertAll(new[]
		{
			 new EquippableitemValues
		 {
			 id = 1,
			 name = "Toothpick",
			 Type = EquippableitemValues.type.polearm,
			 Slot = EquippableitemValues.slot.rightHand,
			 health = 20,
			 damage = 10,
			 damageSpeed = 9,
			 range = 5,
			 characterId = 1,
			 prefabName = "Stick"
		 },
			 new EquippableitemValues
		 {
			 id = 2,
			 name = "Plastic Shield",
			 Type = EquippableitemValues.type.shield,
			 Slot = EquippableitemValues.slot.rightHand,
			 health = 20,
			 damage = 10,
			 damageSpeed = 9,
			 range = 5,
			 characterId = 2,
			 prefabName = "Shield"
		 },
			 new EquippableitemValues
		 {
			 id = 3,
			 name = "Laser Rifle 2000",
			 Type = EquippableitemValues.type.rifle,
			 Slot = EquippableitemValues.slot.rightHand,
			 health = 20,
			 damage = 10,
			 damageSpeed = 9,
			 range = 20,
			 characterId = 3,
			 prefabName = "Rifle"
		 },
			 new EquippableitemValues
		 {
			 id = 4,
			 name = "Rifle of the Git Master Rasmus",
			 Type = EquippableitemValues.type.rifle,
			 Slot = EquippableitemValues.slot.rightHand,
			 health = 25,
			 damage = 15,
			 damageSpeed = 15,
			 range = 15,
			 prefabName = "Rifle"
		 },
			 new EquippableitemValues
		 {
			 id = 5,
			 name = "Mighty power Stick",
			 Type = EquippableitemValues.type.polearm,
			 Slot = EquippableitemValues.slot.rightHand,
			 health = 20,
			 damage = 20,
			 damageSpeed = 9,
			 range = 5,
			 prefabName = "Stick"
		 },
			 new EquippableitemValues
		 {
			 id = 6,
			 name = "Romanian Steel Bar",
			 Type = EquippableitemValues.type.polearm,
			 Slot = EquippableitemValues.slot.rightHand,
			 health = 20,
			 damage = 25,
			 damageSpeed = 9,
			 range = 2,
			 prefabName = "Stick"
		 }
		});
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
		});

	}
	#region character methods
	public IEnumerable<CharacterValues> GetFellowshipValues()
	{
		return _connection.Table<CharacterValues>();
	}

	//TODO generalize this function with any number of fellowship and with rotation
	public GameObject GetPlayerFellowshipInPosition(Transform fellowshipLocation)
	{
		CharacterSpawner[] spawners = fellowshipLocation.gameObject.GetComponentsInChildren<CharacterSpawner>().ToArray();
		GameObject fellowship = new GameObject("PlayerFellowship");
		if (spawners.Length == 4)
		{
			//istantiate player
			GameObject daniel = GenerateCharacterByName("Daniel", spawners[0].transform.position, spawners[0].transform.rotation);
			daniel.transform.parent = fellowship.transform;
			//istantiate fellows and parent them to player
			GameObject john = GenerateCharacterByName("John", spawners[1].transform.position, spawners[1].transform.rotation);
			john.transform.parent = fellowship.transform;
			GameObject nicolai = GenerateCharacterByName("Nicolai", spawners[2].transform.position, spawners[2].transform.rotation);
			nicolai.transform.parent = fellowship.transform;
			GameObject peter = GenerateCharacterByName("Peter", spawners[3].transform.position, spawners[3].transform.rotation);
			peter.transform.parent = fellowship.transform;
		}
		else
		{
			print("Missing some character spawners for the player and/or fellows!");
		}


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
		//load character prefab, weapons prefab and attach them
		//print(StringResources.characterPrefabsPath + charValues.prefabName);
		//load prefab
		GameObject characterGameObject = Instantiate(Resources.Load(StringResources.characterPrefabsPath + charValues.prefabName), position, rotation) as GameObject;
		//assign values to prefab
		characterGameObject.GetComponent<Character>().init(charValues);
		//spawn weapons 
		List<GameObject> equips = GenerateEquippableItemsFromValues(GetCharacterEquippedItemsValues(charValues.id)) as List<GameObject>; //TODO continue here

		equipItemsToCharacter(equips, characterGameObject.GetComponent<Character>());

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
		GameObject currentEquip = new GameObject();
		foreach (EquippableitemValues values in equipValues)
		{
			currentEquip = Instantiate(Resources.Load(StringResources.equippableItemsPrefabsPath + values.prefabName)) as GameObject;

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
				//checking if another item is equipped in the item slot
				if (character.equippableSpots[currentEquipValues.Slot].GetComponentInChildren<EquippableItem>() != null)
				{
					//if thats the case, remove the values and remove the old object
					detatchItemFromCharacter(currentEquipValues.Slot, character);
				}
				//parent and position the item on the appropriate slot
				equip.transform.parent = character.equippableSpots[currentEquipValues.Slot]; equip.transform.localPosition = Vector3.zero;
				//add the new item values
				//to the character prefab
				character.health += currentEquipValues.health;
				character.damage += currentEquipValues.damage;
				character.damageSpeed = currentEquipValues.damageSpeed;
				character.range = currentEquipValues.range;
				//into the database
				currentEquipValues.characterId = character.characterBaseValues.id;
				_connection.Update(currentEquipValues);

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
		{   //removing from database
			_connection.Delete(itemToDetatch.itemValues);
			//removing from scene
			Destroy(itemToDetatch.transform.gameObject);
		}
	}


	public GameObject GenerateNewEquippableItemFromValues(EquippableitemValues values)
	{
		values.id = _connection.Insert(values);

		return GenerateEquippableItemsFromValues(new List<EquippableitemValues>() { values }).FirstOrDefault();


	}

	public IEnumerable<EquippableitemValues> GetEquippableItemsValuesFromInventory()
	{
		List<EquippableitemValues> itemsValues = new List<EquippableitemValues>();

		string query =
			"SELECT itemValues.* FROM EquippableitemValues AS itemValues JOIN InventoryItem AS inventory ON itemValues.id = inventory.deferredId WHERE inventory.Type = " + (int)InventoryItem.type.equippable;

		itemsValues = _connection.Query<EquippableitemValues>(query);


		return itemsValues;
	}

	#endregion

}
