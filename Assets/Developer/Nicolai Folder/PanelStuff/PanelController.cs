using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour, IPointerClickHandler
{

    public GameObject upgradePanel;
    public GameObject soldierPanel;
    //public List<GameObject> panelList= new List<GameObject>();
    public GameObject backgroundPanel;
    public bool panelIsActive = false;

    public void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("hi");
        if (gameObject.CompareTag("Tent") && upgradePanel.activeSelf == false)
        {
            backgroundPanel.SetActive(true);
            upgradePanel.SetActive(true);
            panelIsActive = true;
            print("activating panel1");
        }

        if (gameObject.CompareTag("Friendly") || gameObject.CompareTag("Player") && soldierPanel.activeSelf == false)
        {
            backgroundPanel.SetActive(true);
            soldierPanel.SetActive(true);
            print("activating panel2");
            panelIsActive = true;
        }

    }

}