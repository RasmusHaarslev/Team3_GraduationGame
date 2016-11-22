using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TutorialCloseCharPanel : MonoBehaviour
{
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
