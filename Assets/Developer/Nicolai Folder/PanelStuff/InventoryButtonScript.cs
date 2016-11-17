using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryButtonScript : MonoBehaviour, IPointerClickHandler
{
    PanelScript panelScript;

    void Start()
    {
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        panelScript.panelList[4].SetActive(true);
        panelScript.panelList[5].SetActive(true);
    }

}
