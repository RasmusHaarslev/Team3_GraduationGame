using UnityEngine;
using System.Collections;

public class GoToLevelSelection : MonoBehaviour {

    public GameObject levelSelectionPanel;

    public void GoToCamp()
    {
        //GameController.Instance.LoadScene("LevelSelection");
        levelSelectionPanel.SetActive(true);
    }
}
