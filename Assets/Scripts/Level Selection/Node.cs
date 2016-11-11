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
    public const int _MAXCAMPS = 6;

    /* UNIQUE IDENTIFIER */
    public int NodeId;

    /* ROW LEVEL (DIFFICULTY) */
    public int Level;

    /* FOOD COST TO GO TO THIS LEVEL */
    public int TravelCost;

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
    public int coinAmount;

    /* AMOUNT OF ITEM DROPS */
    public int itemDropAmount;

    /* BOOLEAN CHECKING IF LEVEL IS DONE */
    public bool isCleared = false;
    public bool isScouted = false;
    public bool canPlay = false;
    #endregion

    public List<Sprite> activationImages = new List<Sprite>();

    /* ROADS FROM THIS NODE */
    public List<Link> Links = new List<Link>();

    #region SETUP FUNCTIONS

    public void OnCreate(int id)
    {
        // If root set playable to true
        if (id == 1)
        {
            canPlay = true;
        }
        NodeId = id;
        GetComponent<Button>().onClick.AddListener(BeginLevel);
        SetupCampsForThisNode();
        SetupResourceForThisNode();
        SetupImage();
    }

    /// <summary>
    /// NON-UNIFORM DISTRIBUTION OF CAMPS INSIDE THE LEVEL (STILL RANDOM)
    /// </summary>
    void SetupCampsForThisNode()
    {
        int noCamps = Random.Range(1, _MAXCAMPS);
        CampsInNode = noCamps;

        IEnumerable<int> rangeWolves = Enumerable.Range(0, probabilityWolves);
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
        }
    }

    void SetupResourceForThisNode()
    {
        // FOOD NEED TO BE DEPENDENT OF THE TOTAL COST IN FOOD IT WILL DEMAND TO GO HERE.
        foodAmount = 10;

        // COIN COULD BE A SPAN OVER LIKE 10 ROWS THERE WILL DROP 3 COINS 
        coinAmount = 10;
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

    void BeginLevel()
    {
        /*           
               public int Level;
               public int wolveCamps;
               public int tribeCamps;
               public int choiceCamps;
               public int foodAmount;
               public int coinAmount;
               public int itemDropAmount;
         */

        SetupImage();

        Debug.Log(canPlay);
        Debug.Log(isCleared);

        if (canPlay && !isCleared)
        {
            if (isScouted) { 
                Debug.Log("Show Panel - Can Play - Is Scouted");
            } else {
                Debug.Log("Show Panel - Can Play - Not Scouted");
            }
        } else if (!canPlay)
        {
            if (isScouted)
            {
                Debug.Log("Show Panel - Cannot Play - Is Scouted");
            }
            else
            {
                Debug.Log("Show Panel - Cannot Play - Not Scouted");
            }
        } else
        {
            Debug.Log("Show Panel - You have cleared this level");
        }

        foreach (var link in Links)
        {
            if(link.Hierarchy)
            {
                Debug.Log("Node Id : " + link.From.name + " Has Parent : " + link.To.name);
                Debug.Log("Parent IsClear : " + link.To.GetComponent<Node>().isCleared + " Parent CanPlay : " + link.To.GetComponent<Node>().canPlay);
            } else
            {
                Debug.Log("Node Id : " + link.From.name + " Has Child : " + link.To.name);
                Debug.Log("Child IsClear : " + link.To.GetComponent<Node>().isCleared + " Child CanPlay : " + link.To.GetComponent<Node>().canPlay);
            }
        }
            
        /*PlayerPrefs.SetInt("LevelDifficulty", Level);
        PlayerPrefs.SetInt("WolveCamps", wolveCamps);
        PlayerPrefs.SetInt("TribeCamps", tribeCamps);
        PlayerPrefs.SetInt("ChoiceCamps", choiceCamps);
        PlayerPrefs.SetInt("FoodAmount", foodAmount);
        PlayerPrefs.SetInt("CoinAmount", coinAmount);
        PlayerPrefs.SetInt("ItemDropAmount", itemDropAmount);

        if (SceneTransistion.instance != null)
        {
            SceneTransistion.instance.LoadScene(2);
        }
        else {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        */
    }

    #region Get Functions for this node
    void SetupImage()
    {
        if (isCleared)
        {
            GetComponent<Image>().sprite = activationImages[0];
            GetComponent<Image>().color = Color.green;
        } else if (!isCleared && canPlay)
        {
            GetComponent<Image>().sprite = activationImages[0];
        } else
        {
            GetComponent<Image>().sprite = activationImages[1];
        }
    }

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
    #endregion
}

