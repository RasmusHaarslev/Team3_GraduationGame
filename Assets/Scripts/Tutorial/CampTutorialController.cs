using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampTutorialController : MonoBehaviour
{

    public List<GameObject> panelList = new List<GameObject>();
    private int currentPanel;
    PanelScript panelScript;

    // Use this for initialization
    void Start()
    {
        currentPanel = 0;
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
    }

    public void NextPanel()
    {
        panelList[currentPanel].SetActive(false);
        panelList[++currentPanel].SetActive(true);
    }

    public void OpenLevelSelect()
    {
        panelScript.panelList[2].SetActive(true);
    }

    public void ClickHunter()
    {
        GameObject hunter = GameObject.FindGameObjectsWithTag("Friendly")[1];

        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        if (panelScript.panelList[1].activeSelf == false)
        {
            panelScript.UpdateSoldierStats(hunter);
            panelScript.ActivateCamera(hunter);
            panelScript.panelList[3].SetActive(true);
            panelScript.panelList[1].SetActive(true);
        }
    }

    public void ClickItem()
    {
        DataService dataService = new DataService(StringResources.databaseName);
        List<EquippableitemValues> inventoryItems = new List<EquippableitemValues>(dataService.GetEquippableItemsValuesFromInventory());

        if (inventoryItems[0].Type == EquippableitemValues.type.rifle)
            Manager_Audio.PlaySound(Manager_Audio.play_pickRiffle, gameObject);

        else if (inventoryItems[0].Type == EquippableitemValues.type.polearm)
            Manager_Audio.PlaySound(Manager_Audio.play_pickSpear, gameObject);

        else if (inventoryItems[0].Type == EquippableitemValues.type.shield)
            Manager_Audio.PlaySound(Manager_Audio.play_pickShield, gameObject);

        Object.FindObjectOfType<PanelScript>().AssignWeaponToSoldier(inventoryItems[0]);
    }

    public void CloseCharPanel()
    {
        GameObject centralPanel = GameObject.Find("CentralPanels");
        GameObject soldierPanel = GameObject.Find("SoldierPanel");

        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        if (soldierPanel.activeSelf && centralPanel.activeSelf)
        {
            soldierPanel.SetActive(false);
            centralPanel.SetActive(false);
        }
    }
}