using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GoToLevelSelection : MonoBehaviour {

    public GameObject levelSelectionPanel;

    private bool winAnimation;

    void OnEnable()
    {
        EventManager.Instance.StartListening<LevelCleared>(ClearedLevel);
    }    

    void OnDisable()
    {
        EventManager.Instance.StopListening<LevelCleared>(ClearedLevel);
    }

    public void ClearedLevel(GameEvent e)
    {
        winAnimation = true;
    }


    public void GoToCamp()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_openMap, gameObject);
        levelSelectionPanel.SetActive(true);

        if (winAnimation) { 
            WinAnimateMap();
            winAnimation = !winAnimation;
        } else
        {
            LoseAnimateMap();
        }
    }

    void WinAnimateMap()
    {
        GameObject nodeCleared = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];
        Node nodeScript = nodeCleared.GetComponent<Node>();

        // MAKE A NICE ANIMATION ON NODE THAT WE DID CLEAR

        nodeScript.isCleared = true;

        if (nodeScript.isCleared)
        {
            nodeScript.SetupImage();
            nodeScript.SetupUIText();

            foreach (var nodes in nodeScript.Links.Select(l => l.To).ToList())
            {
                nodes.GetComponent<Node>().canPlay = true;
                nodes.GetComponent<Node>().SetupImage();
            }
        }
    }

    void LoseAnimateMap()
    {
        GameObject nodeCleared = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];
        Node nodeScript = nodeCleared.GetComponent<Node>();

        // MAKE A NICE ANIMATION ON NODE THAT WE DID NOT CLEAR
    }
}
