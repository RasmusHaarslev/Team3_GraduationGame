using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewSoldierConfirmationScript : MonoBehaviour
{
    public GameObject backgroundConfirmationPanel;
    public GameObject centralPanel;
    public GameObject yesButton;
    public GameObject noButton;
    PanelScript panelScript;
    GameObject silhouette;

    void Start()
    {
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ActivateConfirmationPanel);
    }

    public void ActivateConfirmationPanel()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_charSel, gameObject);
        backgroundConfirmationPanel.SetActive(true);
        yesButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.AddListener(delegate { backgroundConfirmationPanel.SetActive(false); });
        yesButton.GetComponent<Button>().onClick.AddListener(delegate { backgroundConfirmationPanel.SetActive(false); gameObject.transform.parent.gameObject.SetActive(false); panelScript.SpawnNewSoldier(transform.GetSiblingIndex()); centralPanel.SetActive(false); });
    }
}


