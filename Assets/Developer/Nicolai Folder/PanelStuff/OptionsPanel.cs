using UnityEngine;
using System.Collections;

public class OptionsPanel : MonoBehaviour {

    private bool panelOpen = false;
    public GameObject OptionPanel = null;
    public GameObject BackgroundPanel;

    public void TogglePanelOpen()
    {
        BackgroundPanel.SetActive(true);
        OptionPanel.SetActive(true);
    }

}