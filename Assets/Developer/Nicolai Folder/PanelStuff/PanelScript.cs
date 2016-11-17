using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;

public class PanelScript : MonoBehaviour {

    public List<GameObject> panelList = new List<GameObject>();
    public List<GameObject> soldierStatsList = new List<GameObject>();
    List<GameObject> soldiersList = new List<GameObject>();
    private DataService dataService;
    public Transform solidersSpawnPosition;
    GameObject charactersFellowship;
    public List<Camera> soldierCameraList = new List<Camera>();
    Character currentSoldier;

    void Start()
    {
        dataService = new DataService(StringResources.databaseName);
        dataService.CreateDB();
        charactersFellowship = dataService.GetPlayerFellowshipInPosition(solidersSpawnPosition);
        InitializeSoldiers();
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

    public void UpdateSoldierStats(GameObject soldier)
    {
        currentSoldier = soldier.GetComponent<Character>();
        foreach(var stat in soldierStatsList)
        {        
            if (stat.name == "Type")
            {     
                stat.GetComponent<Text>().text = "Type: " + currentSoldier.characterBaseValues.Type.ToString();
            }
            if (stat.name == "Damage")
            {
                stat.GetComponent<Text>().text = "Damage: " + currentSoldier.damage.ToString();
            }
            if (stat.name == "Soldier Name")
            {
                stat.GetComponent<Text>().text = "Name: " +  currentSoldier.characterBaseValues.name;
            }
            if (stat.name == "Description")
            {
                stat.GetComponent<Text>().text = "Description: " + currentSoldier.characterBaseValues.description;
            }
            if (stat.name == "Health")
            {
                stat.GetComponent<Text>().text = "Health: " + currentSoldier.health.ToString();
            }
            if (stat.name == "Damage Speed")
            {
                stat.GetComponent<Text>().text = "Damage speed: " +  currentSoldier.damageSpeed.ToString();
            }
            if (stat.name == "Range")
            {
                stat.GetComponent<Text>().text = "Range: " + currentSoldier.range.ToString();
            }
            if (stat.name == "Combat Trait")
            {
                stat.GetComponent<Text>().text = "Combat Trait: " + Regex.Replace(currentSoldier.characterBaseValues.combatTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2");
            }
            if (stat.name == "Target Trait")
            {
                stat.GetComponent<Text>().text = "Target Trait: " + Regex.Replace(currentSoldier.characterBaseValues.targetTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2");
            }

        }
            
    }

    public void ActivateInventoryPanel()
    {
        panelList[4].GetComponent< EquippableItemUIListController >().itemsValues = dataService.GetEquippableItemsValuesFromInventory().ToArray();
        panelList[4].SetActive(true);

    }

    public void AssignWeaponToSoldier(EquippableitemValues weaponValues)
    {
        IEnumerable<GameObject> weapon = dataService.GenerateEquippableItemsFromValues(new[] { weaponValues });
        dataService.equipItemsToCharacter(weapon, currentSoldier);
        UpdateSoldierStats(currentSoldier.gameObject);
        panelList[4].SetActive(false);
        panelList[5].SetActive(false);
        panelList[1].SetActive(true);
    }
}