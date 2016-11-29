using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour {

    #region VARIABLES
    public const int _MINCAMPS = 2;
    public const int _MAXCAMPS = 4;

    /* UNIQUE IDENTIFIER */
    public int NodeId;

    /* ROW LEVEL (DIFFICULTY) */
    public int Level;

    /* FOOD COST TO GO TO THIS LEVEL */
    public int TravelCost;
    public int scoutCost;

    /* THE SCENE TO LOAD WHEN PLAYING LEVEL */
    public int sceneSelection;

    /* AMOUNT OF CAMPS */
    public int CampsInNode;
    [HideInInspector]
    public int probabilityWolves;
    [HideInInspector]
    public int probabilityTribes;
    [HideInInspector]
    public int probabilityChoice;
    public int wolveCamps;
    public int tribeCamps;
    public int choiceCamps;

    /* AMOUNT OF RESOURCE DROPS */
    public int foodAmount;
    public int scrapAmount;

    /* AMOUNT OF ITEM DROPS */
    public int itemDropAmount;

    /* BOOLEAN CHECKING IF LEVEL IS DONE */
    public bool isCleared = false;
    public bool isScouted = false;
    public bool canPlay = false;
    #endregion

    #region UI VARIABLES

    public GameObject infoPanel;
    public GameObject unknownPanel;
    public Text txtFood;
    public Text txtScraps;
    public Text txtTribes;
    public Text txtWolves;
    public Text txtIntPoints;

    #endregion

    public List<Sprite> activationImages = new List<Sprite>();

    /* ROADS FROM THIS NODE */
    public List<Link> Links = new List<Link>();

    #region SETUP FUNCTIONS

    public void OnCreate(int id)
    {
        if (id == 1)
        {
            canPlay = true;
        }

        if (wolveCamps == 0 && tribeCamps == 0 && choiceCamps == 0) { 
            SetupCampsForThisNode();
            SetupResourceForThisNode();
        }

        NodeId = id;
        GetComponent<Button>().onClick.AddListener(OpenPopUp);

        SetupImage();
        SetupUIText();
    }

    public void SetupImage()
    {
        if (isCleared)
        {
            GetComponent<Image>().sprite = activationImages[2];
            GetComponent<Image>().color = Color.green;
        }
        else if (!isCleared && canPlay)
        {
            GetComponent<Image>().sprite = activationImages[1];
        }
        else
        {
            GetComponent<Image>().sprite = activationImages[0];
        }
    }

    public void SetupUIText()
    {
        if(isCleared)
        {
            unknownPanel.SetActive(false);
            infoPanel.SetActive(false);
        }
        else if (isScouted)
        {
            unknownPanel.SetActive(false);
            infoPanel.SetActive(true);

            foreach (RectTransform child in transform)
            {
                if (child.name == "InfoPanel")
                {
                    child.gameObject.SetActive(true);

                    GetComponent<Node>().txtFood.text = GetComponent<Node>().foodAmount.ToString();
                    GetComponent<Node>().txtScraps.text = GetComponent<Node>().scrapAmount.ToString();
                    GetComponent<Node>().txtTribes.text = GetComponent<Node>().tribeCamps.ToString();

                    //GetComponent<Node>().txtWolves.text = GetComponent<Node>().wolveCamps.ToString();
                }
            }

        } else
        {
            infoPanel.SetActive(false);
            unknownPanel.SetActive(true);
            txtIntPoints.text = CampsInNode.ToString();
        }
    }

    /// <summary>
    /// NON-UNIFORM DISTRIBUTION OF CAMPS INSIDE THE LEVEL (STILL RANDOM)
    /// </summary>
    void SetupCampsForThisNode()
    {
        int noCamps = Random.Range(_MINCAMPS, _MAXCAMPS+1);
        CampsInNode = noCamps;

        tribeCamps += CampsInNode;

        /*IEnumerable<int> rangeWolves = Enumerable.Range(0, probabilityWolves);
        IEnumerable<int> rangeTribes = Enumerable.Range(probabilityWolves, probabilityTribes);
        IEnumerable<int> rangeChoices = Enumerable.Range(probabilityWolves + probabilityTribes, probabilityChoice);

        int totalRange = rangeWolves.Count() + rangeTribes.Count() + rangeChoices.Count();
     
        for (int i = 0; i < noCamps; i++)
        {
            int selectionNumber = Random.Range(0, totalRange);

            if (rangeWolves.Contains(selectionNumber))
            {
                wolveCamps += 1;
            }
            else if (rangeTribes.Contains(selectionNumber))
            {
                tribeCamps += 1;
            }
            else
            {
                choiceCamps += 1;
            }
        } */
    }

    void SetupResourceForThisNode()
    {
        // FOOD NEED TO BE DEPENDENT OF THE TOTAL COST IN FOOD IT WILL DEMAND TO GO HERE.
        foodAmount = Random.Range(4, 10);
        if (foodAmount < scoutCost*2)
        {
            foodAmount = scoutCost * 2;
        }

        // SCRAP COULD BE A SPAN OVER LIKE 10 ROWS THERE WILL DROP 3 SCRAPS 
        scrapAmount = 10;
    }
    #endregion

    /// <summary>
    /// Create a new arc, connecting this Node to the Node passed in the parameter
    /// Also, it creates the inversed node in the passed node
    /// </summary>
    public GameObject AddLink(GameObject child, bool isParent) {

        Links.Add(new Link {
            From = gameObject,
            To = child,
            Hierarchy = isParent
        });

        if (!child.GetComponent<Node>().Links.Exists(a => a.From == child && a.To == gameObject)) {
            child.GetComponent<Node>().AddLink(gameObject, true);
        }

        return gameObject;
    }

    void OpenPopUp()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_openMap, gameObject);
        EventManager.Instance.TriggerEvent(new SetupPopUp(gameObject));
    }

    #region Get Functions for this node
    public List<Link> GetParents()
    {
        List<Link> parents = new List<Link>();

        foreach (var link in Links)
        {
            if (link.Hierarchy)
            {
                parents.Add(link);
                continue;
            }
        }

        return parents;
    }
    public List<Link> GetChildrens()
    {
        List<Link> childrens = new List<Link>();

        foreach (var link in Links)
        {
            if (!link.Hierarchy)
            {
                childrens.Add(link);
                continue;
            }
        }

        return childrens;
    }
    
    public void TriggerSound(int changeSound)
    {
        // 0 - Lose
        // 1 - Unlock

        // 2 - Clear
        switch(changeSound) {
            case 0:
                Manager_Audio.PlaySound(Manager_Audio.play_lostMap, gameObject);
                break;
            case 1:
                Manager_Audio.PlaySound(Manager_Audio.play_unlockNewMaps, gameObject);
                break;
            case 2:
                Manager_Audio.PlaySound(Manager_Audio.play_clearMap, gameObject);
                break;
        }
    }
    #endregion
}

