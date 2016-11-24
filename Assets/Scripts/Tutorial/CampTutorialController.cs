using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampTutorialController : MonoBehaviour
{

    public List<GameObject> panelList = new List<GameObject>();
    private int currentPanel;
    PanelScript panelScript;
    GameObject levelSelectionGenerator;

    // Use this for initialization
    void Start()
    {
        currentPanel = 0;
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
        levelSelectionGenerator = GameObject.Find("LevelSelectionGenerator");

        if (PlayerPrefs.HasKey("TutorialCompleted"))
            if (PlayerPrefs.GetInt("TutorialCompleted") == 0)
                gameObject.SetActive(true);
    }

    public void NextPanel()
    {
        panelList[currentPanel].SetActive(false);
        panelList[++currentPanel].SetActive(true);
    }

    public void OpenLevelSelect()
    {
        levelSelectionGenerator.GetComponent<GoToLevelSelection>().GoToCamp();
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
        List<EquippableitemValues> inventoryItems = new List<EquippableitemValues>(GameController.Instance._dataService.GetEquippableItemsValuesFromInventory());

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

    public void ChooseLevel()
    {
        GameObject level0 = GameObject.Find("0.0");
        Node node0 = level0.GetComponent<Node>();

        Manager_Audio.PlaySound(Manager_Audio.play_openMap, level0);
        EventManager.Instance.TriggerEvent(new SetupPopUp(level0));
    }

    public void EnterLevel()
    {
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        gameObject.SetActive(false);
        GameController.Instance.LoadLevel();
    }
}