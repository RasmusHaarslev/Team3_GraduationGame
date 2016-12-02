using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OptionsPanel : MonoBehaviour {

    public GameObject OptionPanel = null;
    public GameObject BackgroundPanel;

    void Start()
    {
       
    }

    void OnDisable()
    {
       
    }

    public void TogglePanelOpen()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        if (scene != 2) { 
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
		EventManager.Instance.TriggerEvent(new UIPanelActiveEvent());
        BackgroundPanel.SetActive(true);
        OptionPanel.SetActive(true);
    }

}