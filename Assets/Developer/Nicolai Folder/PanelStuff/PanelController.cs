using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour, IPointerClickHandler
{
    PanelScript panelScript;
    public GameObject levelSelectionGenerator;
    bool isAlreadyOpen = false;

    void Start()
    {
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_unlockNewMaps, gameObject);
        //Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        if (gameObject.CompareTag("Tent") && panelScript.panelList[0].activeSelf == false)
        {
            
            panelScript.panelList[3].SetActive(true);
            panelScript.panelList[0].SetActive(true);
        }

        if (gameObject.CompareTag("Friendly") || gameObject.CompareTag("Player") && panelScript.panelList[1].activeSelf == false)
        {
            Debug.Log(panelScript);
            panelScript.UpdateSoldierStats(gameObject);
            panelScript.ActivateCamera(gameObject);
            panelScript.panelList[3].SetActive(true);
            panelScript.panelList[1].SetActive(true);
        }

        if (gameObject.CompareTag("MapTable") && panelScript.panelList[2].activeSelf == false)
        {
            levelSelectionGenerator.GetComponent<GoToLevelSelection>().GoToCamp();
        }

        if (gameObject.CompareTag("Silhouette") && panelScript.panelList[6].activeSelf == false)
        {
            //if (!panelScript.alreadyGeneratedNewSoldiers)
            //{
            //    panelScript.GetNewSoldiers();
            //    panelScript.alreadyGeneratedNewSoldiers = true;
            //}
            if (!isAlreadyOpen)
            {
                panelScript.GetNewSoldiers();
                isAlreadyOpen = true;
            }
            panelScript.silhouetteGO = gameObject;
            panelScript.panelList[3].SetActive(true);
            panelScript.panelList[6].SetActive(true);
        }

    }

}