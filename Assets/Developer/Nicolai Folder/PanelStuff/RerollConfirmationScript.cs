using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RerollConfirmationScript : MonoBehaviour {

    public GameObject backgroundConfirmationPanel;
    public GameObject centralPanel;
    public GameObject yesButton;
    public GameObject noButton;
    PanelScript panelScript;
    //GameObject silhouette;

    void Start()
    {
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ActivateConfirmationPanel);
    }

    public void ActivateConfirmationPanel()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        backgroundConfirmationPanel.SetActive(true);
        yesButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.AddListener(delegate { backgroundConfirmationPanel.SetActive(false); });
        yesButton.GetComponent<Button>().onClick.AddListener(delegate { panelScript.RerollSoldiers();  backgroundConfirmationPanel.SetActive(false);});
    }
}
