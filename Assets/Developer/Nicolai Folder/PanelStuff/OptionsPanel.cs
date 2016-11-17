using UnityEngine;
using System.Collections;

public class OptionsPanel : MonoBehaviour {

    private bool panelOpen = false;
    public GameObject OptionPanel = null;
    public GameObject BackgroundPanel;

    void Start()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuMusic, gameObject);
        Manager_Audio.PlaySound(Manager_Audio.play_menuAmbience, gameObject);
    }

    void OnDisable()
    {
        Manager_Audio.PlaySound(Manager_Audio.stop_menuMusic, gameObject);
        Manager_Audio.PlaySound(Manager_Audio.stop_menuAmbience, gameObject);
    }

    public void TogglePanelOpen()
    {
        BackgroundPanel.SetActive(true);
        OptionPanel.SetActive(true);
    }

}