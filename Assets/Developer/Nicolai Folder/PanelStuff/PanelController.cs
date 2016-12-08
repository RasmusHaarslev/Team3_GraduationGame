using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour, IPointerClickHandler
{
    PanelScript panelScript;
    public GameObject levelSelectionGenerator;
    //bool isAlreadyOpen = false;

    void Start()
    {
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        if (gameObject.CompareTag("Tent") && panelScript.panelList[0].activeSelf == false)
        {            
            //panelScript.panelList[3].SetActive(true);
            panelScript.panelList[0].SetActive(true);
            var panel = panelScript.panelList[0].GetComponentInChildren<CampUIController>(true).gameObject;
            panel.SetActive(true);
        }

        //if (gameObject.CompareTag("Friendly") || gameObject.CompareTag("Player") && panelScript.panelList[1].activeSelf == false)
        //{
        //    Manager_Audio.PlaySound(Manager_Audio.play_charSel, gameObject);
        //    panelScript.UpdateSoldierStats(gameObject);
        //    panelScript.ActivateCamera(gameObject);
        //    panelScript.panelList[3].SetActive(true);
        //    panelScript.panelList[1].SetActive(true);
        //}
        if (gameObject.CompareTag("Friendly") && panelScript.panelList[1].activeSelf == false)
        {
            Manager_Audio.PlaySound(Manager_Audio.play_charSel, gameObject);
            panelScript.UpdateSoldierStats(gameObject);
            panelScript.ActivateCamera(gameObject);
            panelScript.panelList[3].SetActive(true);
            panelScript.panelList[1].SetActive(true);
        }

        if (gameObject.CompareTag("Player") && panelScript.panelList[7].activeSelf == false)
        {
            Manager_Audio.PlaySound(Manager_Audio.play_charSel, gameObject);
            panelScript.UpdateSoldierStats(gameObject);
            panelScript.ActivateCamera(gameObject);
            panelScript.panelList[3].SetActive(true);
            panelScript.panelList[7].SetActive(true);
        }

        if (gameObject.CompareTag("MapTable") && panelScript.panelList[2].activeSelf == false)
        {
            levelSelectionGenerator.GetComponent<GoToLevelSelection>().GoToCamp();
        }

        if (gameObject.CompareTag("Silhouette") && panelScript.panelList[6].activeSelf == false)
        {
            //if (!isAlreadyOpen)
            //{
            //    panelScript.GetNewSoldiers();
            //    panelScript.silhouettePosList.Add(gameObject.transform);
            //    isAlreadyOpen = true;
            //}
            if (!panelScript.alreadyGeneratedNewSoldiers)
            {
                panelScript.GetNewSoldiers();
                //panelScript.silhouettePosList.Add(gameObject.transform);
                panelScript.alreadyGeneratedNewSoldiers = true;
            }

            panelScript.silhouetteGO = gameObject;
            panelScript.ActivateNewSoldiers(gameObject.transform);
            panelScript.panelList[3].SetActive(true);
            panelScript.panelList[6].SetActive(true);
        }

    }

}