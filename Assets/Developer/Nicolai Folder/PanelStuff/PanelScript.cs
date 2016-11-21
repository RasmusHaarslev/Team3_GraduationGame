using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class PanelScript : MonoBehaviour {

    public List<GameObject> panelList = new List<GameObject>();
    public List<GameObject> soldierStatsList = new List<GameObject>();
    List<GameObject> soldiersList = new List<GameObject>();
    private DataService dataService;
    public Transform solidersSpawnPosition;
    GameObject charactersFellowship;
    public List<Camera> soldierCameraList = new List<Camera>();
    Character currentSoldier;
    public List<NewSoldierList> newSoldierStatsList;
    List<CharacterValues> newCharacterSoldierList = new List<CharacterValues>();

    [Serializable]
    public class NewSoldierList : IEnumerable<GameObject>
    {
        public List<GameObject> newIndividualSoldierStatsList;

        #region Implementation of IEnumerable
        public IEnumerator<GameObject> GetEnumerator()
        {
            return newIndividualSoldierStatsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }

    void Start()
    {
        dataService = new DataService(StringResources.databaseName);
        dataService.CreateDB();
        charactersFellowship = dataService.GetPlayerFellowshipInPosition(solidersSpawnPosition);
        InitializeSoldiers();
        
        //GetNewSoldiers();// this should be called when player clicks on silhouette
    }

    void GetNewSoldiers()
    {
        GameObject Soldier1 = SpawnNewSoldiers();
        GameObject Soldier2 = SpawnNewSoldiers();
        GameObject Soldier3 = SpawnNewSoldiers();
        Soldier1.transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
        Soldier2.transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
        Soldier3.transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
        FillInNewSoldierStats();
    }
    void InitializeSoldiers()
    {
        for(int i = 0; i < charactersFellowship.transform.childCount; i++)
        {
            soldiersList.Add(charactersFellowship.transform.GetChild(i).gameObject);
        }

        foreach (var soldier in soldiersList)
        {
            soldier.AddComponent<PanelController>();
            soldier.GetComponent<NavMeshAgent>().enabled = false;     
        }
        
        soldiersList[1].GetComponent<HunterStateMachine>().enabled = false;
        soldiersList[2].GetComponent<HunterStateMachine>().enabled = false;
        soldiersList[3].GetComponent<HunterStateMachine>().enabled = false;
        soldiersList[0].GetComponent<MoveScript>().enabled = false;
        soldiersList[0].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player");
        soldiersList[1].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
        soldiersList[2].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter2");
        soldiersList[3].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter3");
    }

    #region Cameras
    public void ActivateCamera(GameObject soldier)
    {
        
        if(soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 9)
        {
            soldierCameraList[0].enabled = true;
            DeactivateCamera(0);
        }
        if (soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 10)
        {
            soldierCameraList[1].enabled = true;
            DeactivateCamera(1);
        }
        if (soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 11)
        {
            soldierCameraList[2].enabled = true;
            DeactivateCamera(2);
        }
        if (soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 12)
        {
            soldierCameraList[3].enabled = true;
            DeactivateCamera(3);
        }
    }

    public void DeactivateCamera(int cameraIndex)
    {
        for (int i = 0; i < charactersFellowship.transform.childCount ; i++)
        {
            if(i != cameraIndex)
            {
                soldierCameraList[i].enabled = false;
            }   
        }     
    }
    #endregion

    void FillInNewSoldierStats()
    {
        int i = 0;
        foreach (var newSoldierStats in newSoldierStatsList)
        {
            
            foreach (var stat in newSoldierStats)
            {

                if (stat.name == "Type")
                {
                    stat.GetComponent<Text>().text = "Type: " + newCharacterSoldierList[i].Type.ToString();
                }
                if (stat.name == "Damage")
                {
                    stat.GetComponent<Text>().text = "Damage: " + newCharacterSoldierList[i].damage.ToString();
                }
                if (stat.name == "Soldier Name")
                {
                    stat.GetComponent<Text>().text = "Name: " + newCharacterSoldierList[i].name;
                }
                if (stat.name == "Description")
                {
                    stat.GetComponent<Text>().text = "Description: " + newCharacterSoldierList[i].description;
                }
                if (stat.name == "Health")
                {
                    stat.GetComponent<Text>().text = "Health: " + newCharacterSoldierList[i].health.ToString();
                }
                if (stat.name == "Damage Speed")
                {
                    stat.GetComponent<Text>().text = "Damage speed: " + newCharacterSoldierList[i].damageSpeed.ToString();
                }
                if (stat.name == "Range")
                {
                    stat.GetComponent<Text>().text = "Range: " + newCharacterSoldierList[i].range.ToString();
                }
                if (stat.name == "Combat Trait")
                {
                    stat.GetComponent<Text>().text = "Combat Trait: " + Regex.Replace(newCharacterSoldierList[i].combatTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2");
                }
                if (stat.name == "Target Trait")
                {
                    stat.GetComponent<Text>().text = "Target Trait: " + Regex.Replace(newCharacterSoldierList[i].targetTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2");
                }

            }
            i++;
        }
        newCharacterSoldierList.Clear();
    }

    public void UpdateSoldierStats(GameObject soldier)
    {
        currentSoldier = soldier.GetComponent<Character>();
        foreach(var stat in soldierStatsList)
        {        
            if (stat.name == "Type")
            {     
                stat.GetComponent<Text>().text = currentSoldier.characterBaseValues.Type.ToString();
            }
            if (stat.name == "Damage")
            {
                stat.GetComponent<Text>().text = currentSoldier.damage.ToString();
            }
            if (stat.name == "Soldier Name")
            {
                stat.GetComponent<Text>().text = currentSoldier.characterBaseValues.name;
            }
            if (stat.name == "Description")
            {
                stat.GetComponent<Text>().text = currentSoldier.characterBaseValues.description;
            }
            if (stat.name == "Health")
            {
                stat.GetComponent<Text>().text = currentSoldier.health.ToString();
            }
            if (stat.name == "Damage Speed")
            {
                stat.GetComponent<Text>().text =  currentSoldier.damageSpeed.ToString();
            }
            if (stat.name == "Range")
            {
                stat.GetComponent<Text>().text = currentSoldier.range.ToString();
            }
            if (stat.name == "Combat Trait")
            {
                stat.GetComponent<Text>().text = Regex.Replace(currentSoldier.characterBaseValues.combatTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2");
            }
            if (stat.name == "Target Trait")
            {
                stat.GetComponent<Text>().text = Regex.Replace(currentSoldier.characterBaseValues.targetTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2");
            }

        }
            
    }

    #region InventoryMethods

    public void ActivateInventoryPanel()
    {
        panelList[4].GetComponent< EquippableItemUIListController >().GenerateItemsList(dataService.GetEquippableItemsValuesFromInventory().ToArray());
        panelList[4].SetActive(true);

    }

    public void AssignWeaponToSoldier(EquippableitemValues weaponValues)
    {

        IEnumerable<GameObject> weapon = dataService.GenerateEquippableItemsFromValues(new[] { weaponValues });
        dataService.equipItemsToCharacter(weapon, currentSoldier);
        UpdateSoldierStats(currentSoldier.gameObject);

        //foreach (var camera in soldierCameraList)
        //{
        //    if (camera.enabled == true)
        //    {
        //        if (weaponValues.Type == EquippableitemValues.type.rifle)
        //        {
        //            camera.cullingMask = camera.cullingMask ^= (1 << LayerMask.NameToLayer("Rifle") | 1 << LayerMask.NameToLayer("Player"));
        //        }
        //        if (weaponValues.Type == EquippableitemValues.type.shield)
        //        {
        //            camera.cullingMask ^= (1 << LayerMask.NameToLayer("Shield") | 1 << LayerMask.NameToLayer("Player"));
        //        }
        //        if (weaponValues.Type == EquippableitemValues.type.polearm)
        //        {
        //            camera.cullingMask ^= (1 << LayerMask.NameToLayer("Stick") | 1 << LayerMask.NameToLayer("Player"));
        //        }
        //    }
        //}

        panelList[4].SetActive(false);
        panelList[5].SetActive(false);
        panelList[1].SetActive(true);
    }

    #endregion

    public CharacterValues GenerateNewCharacterValues()
    {
        CharacterValues newCharValues = new CharacterValues();
        
        //generate random name
        String characterName = StringResources.maleNames[UnityEngine.Random.Range(0, StringResources.maleNames.Length)];
        newCharValues.name = characterName;
        //generate stats
        newCharValues = GenerateNewCharacterStats(newCharValues);
        //generate material
        newCharValues.prefabName = StringResources.follower1PrefabName;

        newCharacterSoldierList.Add(newCharValues);
        Debug.Log(newCharacterSoldierList.Count);
        
        return newCharValues;
    }

    CharacterValues GenerateNewCharacterStats(CharacterValues charValues)
    {
        charValues.damage = 5;
        charValues.damageSpeed = 5;
        charValues.health = 5;
        charValues.range = 5;
        charValues.combatTrait = CharacterValues.CombatTrait.BraveFool;
        charValues.targetTrait = CharacterValues.TargetTrait.Loyal;

        return charValues;

    }

    GameObject SpawnNewSoldiers()
    {
        CharacterValues char1Values = GenerateNewCharacterValues();
        return dataService.GenerateCharacterFromValues(char1Values, new Vector3(0, 0, -12));
    }

}