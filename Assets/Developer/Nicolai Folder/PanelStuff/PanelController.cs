using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour, IPointerClickHandler
{
    PanelScript panelScript;

    void Start()
    {
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
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

    }

}