using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour {

    public List<GameObject> panelList = new List<GameObject>();
    public List<GameObject> soldierStatsList = new List<GameObject>();
    List<GameObject> soldiersList = new List<GameObject>();
    private DataService dataService;
    public Transform solidersSpawnPosition;
    GameObject charactersFellowship;

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
        }
 
    }

    public void UpdateSoldierStats(GameObject soldier)
    {
        Character currentSoldier = soldier.GetComponent<Character>();
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
            if (stat.name == "Tier")
            {
                stat.GetComponent<Text>().text = "Tier: " + currentSoldier.characterBaseValues.tier.ToString();
            }
        }
            
    }
}