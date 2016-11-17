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
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        panelScript.ActivateInventoryPanel();
        panelScript.panelList[4].SetActive(true);
    }

}
