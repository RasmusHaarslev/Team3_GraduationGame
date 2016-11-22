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
    public int newCharPoints = 15;
    public GameObject newSoldierSpawnPosition;
    [Range (0f, 1f)]
    public float damagePointsChance = 0.5f;

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
        print("Fellowship retrieved");
        InitializeSoldiers();
        
        GetNewSoldiers();// this should be called when player clicks on silhouette
    }

    void GetNewSoldiers()
    {
        GameObject Soldier1 = GenerateNewHunter();
        GameObject Soldier2 = GenerateNewHunter();
        GameObject Soldier3 = GenerateNewHunter();
        Soldier1.GetComponent<NavMeshAgent>().enabled = false;
        Soldier2.GetComponent<NavMeshAgent>().enabled = false;
        Soldier3.GetComponent<NavMeshAgent>().enabled = false;
        Soldier1.GetComponent<HunterStateMachine>().enabled = false;
        Soldier2.GetComponent<HunterStateMachine>().enabled = false;
        Soldier3.GetComponent<HunterStateMachine>().enabled = false;
        Soldier1.transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
        Soldier2.transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter2");
        Soldier3.transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter3");
        FillInNewSoldierStats();
    }

    void InitializeSoldiers()
    {
        int[] layersIndices = new[] { 9, 10, 11, 12 };


        for(int i = 0; i < charactersFellowship.transform.childCount; i++)
        {
            soldiersList.Add(charactersFellowship.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < soldiersList.Count; i++)
        //foreach (GameObject soldier in soldiersList)
        {
            soldiersList[i].AddComponent<PanelController>();
            soldiersList[i].GetComponent<NavMeshAgent>().enabled = false;     

            
            if(soldiersList[i].GetComponent<Character>().characterBaseValues.Type == CharacterValues.type.Player)
            {
                soldiersList[i].GetComponent<MoveScript>().enabled = false;
            }else
            {
                soldiersList[i].GetComponent<HunterStateMachine>().enabled = false;
            }

            soldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = layersIndices[i];

        }


        
        /*
        soldiersList[1].GetComponent<HunterStateMachine>().enabled = false;
        soldiersList[2].GetComponent<HunterStateMachine>().enabled = false;
        soldiersList[3].GetComponent<HunterStateMachine>().enabled = false;
        soldiersList[0].GetComponent<MoveScript>().enabled = false;
        soldiersList[0].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player");
        soldiersList[1].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
        soldiersList[2].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter2");
        soldiersList[3].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter3");
        */

       

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

    public GameObject GenerateNewHunter()
    {
        CharacterValues newCharValues = GenerateNewCharacterValues();
        GameObject hunter = dataService.GenerateCharacterFromValues(newCharValues, new Vector3(0, 0, -12));
       
        //create new weapon for new soldier
        Array itemValues = Enum.GetValues(typeof(EquippableitemValues.type));
        EquippableitemValues newWeaponValues = GetComponent<WeaposGenerator>().GenerateEquippableItem((EquippableitemValues.type)itemValues.GetValue(UnityEngine.Random.Range(0, itemValues.Length)), 1);
        IEnumerable<GameObject> newWeapon = dataService.GenerateEquippableItemsFromValues(new[] { newWeaponValues });
        Character hunterChar = hunter.GetComponent<Character>();
        //generate  and attach the weapon
        dataService.equipItemsToCharacter(newWeapon, hunterChar);

        return hunter;

    }

    public CharacterValues GenerateNewCharacterValues()
    {
        CharacterValues newCharValues = new CharacterValues();
        //generate prefab 
        newCharValues.prefabName = StringResources.follower1PrefabName;
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        //generate random name
        String characterName;
        if(randomValue > 0.5f)//we create a Male
        {
            characterName = StringResources.maleNames[UnityEngine.Random.Range(0, StringResources.maleNames.Length)];
            newCharValues.isMale = true;
            newCharValues.materialName = StringResources.maleHuntersMaterials[UnityEngine.Random.Range(0, StringResources.maleHuntersMaterials.Length)];
        }
        else //we create a female
        {
            characterName =  StringResources.femaleNames[UnityEngine.Random.Range(0, StringResources.femaleNames.Length)];
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
       

        newCharacterSoldierList.Add(newCharValues);
   
        return newCharValues;
    }

    CharacterValues GenerateNewCharacterStats(CharacterValues charValues)
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        for (int i = 0; i < newCharPoints; i ++)
        {
            if(randomValue < damagePointsChance)
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

}