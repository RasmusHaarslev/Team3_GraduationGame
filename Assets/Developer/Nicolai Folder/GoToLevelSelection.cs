using UnityEngine;
using System.Collections;

public class GoToLevelSelection : MonoBehaviour {

    public GameObject levelSelectionPanel;

    public void GoToCamp()
    {
        levelSelectionPanel.SetActive(true);
    }
}
