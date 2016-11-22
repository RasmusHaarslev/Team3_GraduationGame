using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampTutorialController : MonoBehaviour
{

    public List<GameObject> panelList = new List<GameObject>();
    private int currentPanel;

    // Use this for initialization
    void Start()
    {
        currentPanel = 0;
    }

    public void NextPanel()
    {
        panelList[currentPanel].SetActive(false);
        panelList[++currentPanel].SetActive(true);
    }
}