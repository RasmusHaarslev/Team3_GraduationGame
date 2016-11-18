using UnityEngine;
using System.Collections;

public class GoToLevelSelection : MonoBehaviour {

    public GameObject levelSelectionPanel;

    public void GoToCamp()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_openMap, gameObject);
        //GameController.Instance.LoadScene("LevelSelection");
        levelSelectionPanel.SetActive(true);
    }
}
