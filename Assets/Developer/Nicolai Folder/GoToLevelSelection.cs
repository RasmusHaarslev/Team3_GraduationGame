using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GoToLevelSelection : MonoBehaviour {

    public GameObject levelSelectionPanel;

    private bool winAnimation = false;
    private bool loseAnimation = false;
    private bool nodeCleared;

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
		//this.enabled = false;
	}

	public void ClearedLevel(LevelCleared e)
    {
        nodeCleared = e.isCleared;

        if(nodeCleared)
        {
            winAnimation = true;
        } else if (!nodeCleared)
        {
            loseAnimation = true;
        }
    }

    public void GoToCamp()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_openMap, gameObject);
        levelSelectionPanel.SetActive(true);

        gameObject.GetComponent<LevelSelectionGenerator>().SetScrollPosition(SaveLoadLevels.maxRowsCleared);

        if (winAnimation && nodeCleared) {
            levelSelectionPanel.transform.GetChild(0).GetComponent<Button>().enabled = false;
            WinAnimateMap();
            winAnimation = !winAnimation;
        }

        if (loseAnimation && !nodeCleared)
        {
            levelSelectionPanel.transform.GetChild(0).GetComponent<Button>().enabled = false;
            LoseAnimateMap();
            loseAnimation = !loseAnimation;
        }

        if (SaveLoadLevels.lastNodeCleared != null) { 
            SaveLoadLevels.lastNodeCleared.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    #region WIN ANIMATION
    void WinAnimateMap()
    {
        GameObject nodeCleared = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];

        if (nodeCleared.GetComponent<Node>().isCleared)
        {
            StartCoroutine(initWin(nodeCleared));                 
        }
    }

    IEnumerator initWin(GameObject node)
    {
        Node nodeScript = node.GetComponent<Node>();
        List<GameObject> nodeList = nodeScript.Links.Select(l => l.To).ToList();

        Manager_Audio.PlaySound(Manager_Audio.play_fadeNode, gameObject);
        node.GetComponent<Animator>().SetTrigger("IsCleared");
        yield return new WaitForSeconds(1f);

        nodeScript.SetupUIText();
        node.transform.GetChild(2).gameObject.SetActive(true);

        foreach (var childNode in nodeList)
        {
            if (!childNode.GetComponent<Node>().isCleared && !childNode.GetComponent<Node>().isOpen)
            {
                childNode.GetComponent<Node>().isOpen = true;
                Manager_Audio.PlaySound(Manager_Audio.play_fadeNode, gameObject);
                childNode.GetComponent<Animator>().SetTrigger("IsUnlocked");
                yield return new WaitForSeconds(1f);
            }
        }

        levelSelectionPanel.transform.GetChild(0).GetComponent<Button>().enabled = true;
        EventManager.Instance.TriggerEvent(new SaveLevelsToXML());
    }
    #endregion

    #region LOSE ANIMATION
    void LoseAnimateMap()
    {
        GameObject nodeLost = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];
        Debug.Log(nodeLost.name);
        StartCoroutine(initLose(nodeLost));
    }

    IEnumerator initLose(GameObject node)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_fadeNode, gameObject);
        node.GetComponent<Animator>().SetTrigger("IsLost");
        yield return new WaitForSeconds(1f);
        levelSelectionPanel.transform.GetChild(0).GetComponent<Button>().enabled = true;
    }
    #endregion
}
