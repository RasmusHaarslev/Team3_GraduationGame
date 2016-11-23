﻿using UnityEngine;
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
        Debug.Log(winAnimation);
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
        } else
        {
            if (SaveLoadLevels.lastNodeCleared != null) { 
                SaveLoadLevels.lastNodeCleared.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    void WinAnimateMap()
    {
        GameObject nodeCleared = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];
        Node nodeScript = nodeCleared.GetComponent<Node>();

        nodeCleared.transform.GetChild(2).gameObject.SetActive(true);

        // MAKE A NICE ANIMATION ON NODE THAT WE DID CLEAR        

        nodeScript.isCleared = true;

        if (nodeScript.isCleared)
        {
            StartCoroutine(initWin(nodeCleared));
            //nodeScript.SetupImage();
            nodeScript.SetupUIText();

            foreach (var nodes in nodeScript.Links.Select(l => l.To).ToList())
            {
                nodes.GetComponent<Node>().canPlay = true;
                nodes.GetComponent<Node>().SetupImage();
            }
        }
    }

    IEnumerator initWin(GameObject node)
    {
        //node.GetComponent<Animator>().SetBool("IsCleared", true);

        yield return new WaitForSeconds(1);
    }

    void LoseAnimateMap()
    {
        GameObject nodeCleared = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];
        Node nodeScript = nodeCleared.GetComponent<Node>();

        // MAKE A NICE ANIMATION ON NODE THAT WE DID NOT CLEAR
    }
}
