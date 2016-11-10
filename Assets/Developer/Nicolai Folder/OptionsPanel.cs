using UnityEngine;
using System.Collections;

public class OptionsPanel : MonoBehaviour {


    private bool panelOpen = false;
    public GameObject OptionPanel = null;

    public void TogglePanelOpen()
    {
        panelOpen = !panelOpen;
        SetPanelVisible();
    }

    public void SetPanelVisible()
    {
        if(OptionPanel != null)
        {
            OptionPanel.SetActive(panelOpen);
        }   
    }

}