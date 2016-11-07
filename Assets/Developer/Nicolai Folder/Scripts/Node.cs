using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Node : MonoBehaviour
{

    LineRenderer lr;

    #region VARIABLES
    public const int _MAXCAMPS = 6;

    /* UNIQUE IDENTIFIER */
    public int NodeId;

    /* ROW LEVEL (DIFFICULTY) */
    public int Level;

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
    #endregion

    /* ROADS FROM THIS NODE */
    public List<Link> Links = new List<Link>();

    #region SETUP FUNCTIONS

    public void OnCreate(int id)
    {
        NodeId = id;
        lr = GetComponentInChildren<LineRenderer>();
        GetComponent<Button>().onClick.AddListener(BeginLevel);
        SetupCampsForThisNode();
        SetupResourceForThisNode();
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
    public GameObject AddLink(GameObject child, int w)
    {

        Links.Add(new Link
        {
            From = gameObject,
            To = child,
            FoodCost = w
        });

        // MAKE NICE LINE HERE
        SetupLine(gameObject, child);

        if (!child.GetComponent<Node>().Links.Exists(a => a.From == child && a.To == this))
        {
            child.GetComponent<Node>().AddLink(gameObject, w);
        }

        return gameObject;
    }

    void SetupLine(GameObject stage1, GameObject stage2)
    {
        lr.SetVertexCount(2);
        lr.SetWidth(0.3f, 0.3f);
        StartCoroutine(AnimateLineBetween(stage1.GetComponent<RectTransform>(), stage2.GetComponent<RectTransform>()));
    }

    IEnumerator AnimateLineBetween(RectTransform a, RectTransform b)
    {
        // set first point
        lr.SetPosition(0, a.localPosition);
        // initialize second point
        lr.SetPosition(1, a.localPosition);

        // the distance (and direction) between the two points
        Vector3 distance = b.anchoredPosition3D - a.anchoredPosition3D;
        for (float i = 0; i < 1; i += 10 / 200)
        {
            // each frame, advance a fraction of the way
            lr.SetPosition(1, distance * i);
            yield return null;
        }
    }

    void BeginLevel()
    {
        Debug.Log("Load Scene : " + sceneSelection);
    }
}

