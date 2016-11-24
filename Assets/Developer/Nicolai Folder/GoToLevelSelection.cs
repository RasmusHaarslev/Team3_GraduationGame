using UnityEngine;
using UnityEngine.UI;
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

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	public void ClearedLevel(GameEvent e)
    {
        winAnimation = true;
    }


    public void GoToCamp()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_openMap, gameObject);
        levelSelectionPanel.SetActive(true);

        gameObject.GetComponent<LevelSelectionGenerator>().SetScrollPosition(SaveLoadLevels.maxRowsCleared);

        if (winAnimation) { 
            WinAnimateMap();
            winAnimation = !winAnimation;
        } else if (!winAnimation)
        {
          //  LoseAnimateMap();
        }

        if (SaveLoadLevels.lastNodeCleared != null) { 
            SaveLoadLevels.lastNodeCleared.transform.GetChild(2).gameObject.SetActive(true);
        }

    }

    void WinAnimateMap()
    {
        GameObject nodeCleared = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];
        Node nodeScript = nodeCleared.GetComponent<Node>();

        if (nodeScript.isCleared)
        {
            StartCoroutine(initWin(nodeCleared));

            nodeScript.SetupUIText();
            nodeCleared.transform.GetChild(2).gameObject.SetActive(true);

            foreach (var nodes in nodeScript.Links.Select(l => l.To).ToList())
            {
                StartCoroutine(initUnlock(nodes));
            }
        }
    }

    IEnumerator initWin(GameObject node)
    {
        node.GetComponent<Animator>().SetTrigger("IsCleared");
        yield return new WaitForSeconds(2);   
    }

    IEnumerator initUnlock(GameObject node)
    {
        node.GetComponent<Animator>().SetTrigger("IsUnlocked");
        yield return new WaitForSeconds(1);
    }

    void LoseAnimateMap()
    {
        GameObject nodeLost = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];

        StartCoroutine(initLose(nodeLost));
    }

    IEnumerator initLose(GameObject node)
    {
        node.GetComponent<Animator>().SetTrigger("IsLost");
        yield return new WaitForSeconds(2);
    }
}
