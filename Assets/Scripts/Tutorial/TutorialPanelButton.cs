using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TutorialPanelButton : MonoBehaviour
{
    PanelScript panelScript;

    void Start()
    {
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
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
}