using UnityEngine;
using System.Collections;

public class OptionsPanel : MonoBehaviour {


    private bool panelOpen = false;
    public GameObject OptionPanel;

    public void TogglePanelOpen()
    {
        panelOpen = !panelOpen;
        SetPanelVisible();
    }

    public void SetPanelVisible()
    {
        OptionPanel.SetActive(panelOpen);
    }

}
