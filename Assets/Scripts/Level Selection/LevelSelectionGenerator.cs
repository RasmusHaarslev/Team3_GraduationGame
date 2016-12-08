using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelSelectionGenerator : MonoBehaviour
{

    #region VARIABLES
    public GameObject scrollRect;
    public GameObject scrollingGrid;
    public GameObject dropDown;

    public GameObject rowPrefab;
    public GameObject rowImagePrefab;
    public GameObject nodePrefab;

    public int amountOfRows = 0;

    [Tooltip("Determine the max amonut of items that drop within a single level")]
    [Range(2, 10)]
    public int MaxItemDropAmount;

    [Tooltip("Lowest amount of food to use to go to a level")]
    [Range(0, 10)]
    public int LowestTravelCost;

    [Tooltip("Highest amount of food to use to go to a level")]
    [Range(0, 10)]
    public int HighestTravelCost;

    [Tooltip("Lowest amount of food to use to scout a level")]
    [Range(0, 10)]
    public int LowestScoutCost;

    [Tooltip("Highest amount of food to use to scout a level")]
    [Range(0, 10)]
    public int HighestScoutCost;

    int numOfLastLevels = 1;
    int totalAmountRows;
    int nodeCounter = 0;
    int numOfParents = 0;
    int goldteethDrop = 2;
    #endregion

    public Dictionary<int, List<GameObject>> nodesInRows = new Dictionary<int, List<GameObject>>();
    List<GameObject> nodes = new List<GameObject>();

    void OnEnable()
    {
        EventManager.Instance.StartListening<SaveLevelsToXML>(SaveDict);
        Time.timeScale = 1;
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<SaveLevelsToXML>(SaveDict);
    }

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

    void Awake()
    {
        if (PlayerPrefs.GetInt("LevelsInstantiated") != 1)
        {
            InstantiateRows(amountOfRows);
            PlayerPrefs.SetInt("LevelsInstantiated", 1);
        }
        else
        {
            // Need to read from an external file for this to work.
            nodesInRows = SaveLoadLevels.LoadLevels();

            totalAmountRows = 0;
            numOfLastLevels = nodesInRows.OrderBy(key => key.Key).Last().Value.Count;
            LoadRows();

            if (PlayerPrefs.GetInt("LevelResult") != 0)
            {
                EventManager.Instance.TriggerEvent(new LevelCleared(true));
                GameObject nodeCleared = SaveLoadLevels.AllLevelsLoaded[PlayerPrefs.GetInt("NodeId")];

                Node nodeScript = nodeCleared.GetComponent<Node>();     

                nodeScript.isCleared = true;

                foreach (var nodes in nodeScript.Links.Select(l => l.To).ToList())
                {
                    nodes.GetComponent<Node>().canPlay = true;
                }

                InstantiateRows(1);

                PlayerPrefs.SetInt("LevelResult", 0);
            } else
            {
                EventManager.Instance.TriggerEvent(new LevelCleared(false));
            }

        }

        SaveDict(new SaveLevelsToXML());
        EventManager.Instance.TriggerEvent(new SaveLevelsToXML());
    }

    public void LoadRows()
    {
        foreach (KeyValuePair<int, List<GameObject>> entry in nodesInRows)
        {
            GameObject row = Instantiate(rowPrefab);
            ResetTransform(row, scrollingGrid);
            row.name = (entry.Key).ToString();

            float startX = 0f;
            float increaseX = 0f;

            switch (entry.Value.Count)
            {
                case 1:
                    startX = 0;
                    increaseX = 0;
                    break;
                case 2:
                    startX = -178.3f;
                    increaseX = 356.6f;
                    break;
                case 3:
                    startX = -356.6f;
                    increaseX = 356.6f;
                    break;
                case 4:
                    startX = -535f;
                    increaseX = 356.6f;
                    break;
            }


            for (int j = 0; j < entry.Value.Count; j++)
            {
                GameObject newNode = entry.Value[j];
                ResetTransform(newNode, row);

                newNode.name = (totalAmountRows) + "." + j;
                newNode.transform.localPosition = new Vector3(startX, newNode.transform.localPosition.y - 84f, 0);

                nodeCounter++;
                startX += increaseX;
            }

            if (entry.Key > 0)
            {
                GameObject imageRow = Instantiate(rowImagePrefab);
                ResetTransform(imageRow, row);
                imageRow.transform.localPosition = new Vector3(0, 100f, 0);
                imageRow.GetComponent<AddImageRow>().InsertImage(entry.Value.Count, nodesInRows[entry.Key - 1].Count);
            }

            totalAmountRows++;
        }
    }

    /// <summary>
    /// Building the Rows and randomly selection how many nodes pr. row
    /// </summary>
    /// <param name="rowAmount"></param>
    public void InstantiateRows(int rowAmount)
    {

        for (int i = 0; i < rowAmount; i++)
        {

            GameObject row = Instantiate(rowPrefab);
            ResetTransform(row, scrollingGrid);
            row.name = (totalAmountRows).ToString();

            numOfParents = numOfLastLevels;

            if (nodeCounter > 0)
            {
                numOfLastLevels = randomController(numOfLastLevels);
            }
            else
            {
                numOfLastLevels = 1;
            }

            float startX = 0f;
            float increaseX = 0f;

            switch (numOfLastLevels)
            {
                case 1:
                    startX = 0;
                    increaseX = 0;
                    break;
                case 2:
                    startX = -178.3f;
                    increaseX = 356.6f;
                    break;
                case 3:
                    startX = -356.6f;
                    increaseX = 356.6f;
                    break;
                case 4:
                    startX = -535f;
                    increaseX = 356.6f;
                    break;
            }

            List<GameObject> rowNodes = new List<GameObject>();

            for (int j = 0; j < numOfLastLevels; j++)
            {
                nodeCounter++;

                GameObject newNode = Instantiate(nodePrefab);
                ResetTransform(newNode, row);

                if (totalAmountRows % 10 == 0)
                {
                    Debug.Log("RESET GOLD TEETH" + " ROW COUNTS : " + totalAmountRows);
                    goldteethDrop = 2;                    
                }

                SetupValuesInNode(newNode);

                newNode.name = (totalAmountRows) + "." + j;
                newNode.transform.localPosition = new Vector3(startX, newNode.transform.localPosition.y - 84f, 0);

                newNode.GetComponent<Node>().OnCreate(nodeCounter);

                rowNodes.Add(newNode);
                nodes.Add(newNode);

                if (nodeCounter > 1)
                {
                    int counter = 0;
                    foreach (var node in nodesInRows[totalAmountRows - 1])
                    {
                        SetupChildrenNodes(node, newNode, counter, j);
                        counter++;
                    }
                }

                startX += increaseX;
            }

            nodesInRows.Add(totalAmountRows, rowNodes);

            if (nodeCounter > 1)
            {
                GameObject imageRow = Instantiate(rowImagePrefab);
                ResetTransform(imageRow, row);
                imageRow.transform.localPosition = new Vector3(0, 100f, 0);
                imageRow.GetComponent<AddImageRow>().InsertImage(numOfLastLevels, numOfParents);
            }

            totalAmountRows += 1;
        }

        SaveLoadLevels.SaveLevels(nodesInRows);
    }

    /// <summary>
    /// Link current node with children nodes (newNode) based on a counter where 1 is most left- then 4 is most right.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="newNode"></param>
    /// <param name="counter"></param>
    void SetupChildrenNodes(GameObject node, GameObject newNode, int parentFromLeft, int childFromLeft)
    {
        // NumOfParent is equal to the amount of nodes in the current level
        // NumOfLasLevels is equal to the amount of children nodes
        if (numOfParents == 1 || numOfLastLevels == 1)
        {
            node.GetComponent<Node>().AddLink(newNode, false);
        }
        else if (numOfParents == 4)
        {
            switch (numOfLastLevels)
            {
                case 3:
                    if (parentFromLeft == 0 && childFromLeft == 0)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 1 && childFromLeft < 2)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 2 && childFromLeft > 0 && childFromLeft < 3)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 3 && childFromLeft == 2)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    break;
                case 2:
                    if (parentFromLeft < 2 && childFromLeft == 0)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft > 1 && childFromLeft == 1)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    break;
            }
        }
        else if (numOfParents == 3)
        {
            switch (numOfLastLevels)
            {
                case 4:
                    if (parentFromLeft == 0 && childFromLeft < 2)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 1 && childFromLeft > 0 && childFromLeft < 3)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 2 && childFromLeft > 1 && childFromLeft < 4)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    break;
                case 2:
                    if (parentFromLeft == 0 && childFromLeft == 0)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 1)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 2 && childFromLeft > 0)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    break;
            }
        }
        else if (numOfParents == 2)
        {
            switch (numOfLastLevels)
            {
                case 4:
                    if (parentFromLeft == 0 && childFromLeft < 2)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 1 && childFromLeft > 1 && childFromLeft < 4)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    break;
                case 3:
                    if (parentFromLeft == 0 && childFromLeft < 2)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    else if (parentFromLeft == 1 && childFromLeft > 0)
                    {
                        node.GetComponent<Node>().AddLink(newNode, false);
                    }
                    break;
            }
        }
    }

    void SetupValuesInNode(GameObject node)
    {
        bool containTeeth = UnityEngine.Random.Range(0, 3) == 2 ? true : false;

        node.GetComponent<Node>().Level = totalAmountRows;
        node.GetComponent<Node>().TravelCost = UnityEngine.Random.Range(LowestTravelCost, HighestTravelCost);
        node.GetComponent<Node>().scoutCost = UnityEngine.Random.Range(LowestScoutCost, HighestScoutCost);
        node.GetComponent<Node>().itemDropAmount = UnityEngine.Random.Range(1, MaxItemDropAmount);

        if (containTeeth && goldteethDrop > 0) {
            int teethAmount = UnityEngine.Random.Range(1, goldteethDrop+1);
            node.GetComponent<Node>().goldTeethAmount = teethAmount;
            goldteethDrop -= teethAmount;
            Debug.Log("GOLD LEFT : " + goldteethDrop + " Row : " + amountOfRows + " Has GOLDTEETH!!!!!!!!!!!!!!!!!!!!!!! : " + teethAmount);
        }
    }

    #region Helper function
    void printDict(Dictionary<int, List<GameObject>> dict)
    {
        foreach (KeyValuePair<int, List<GameObject>> kvp in dict)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value.Count));
        }
    }

    /// <summary>
    /// Return a new random integer between 1 and 4 different from the last
    /// </summary>
    int randomController(int oldNumber)
    {
        int a = UnityEngine.Random.Range(1, 5);

        if (a == oldNumber)
        {
            return randomController(oldNumber);
        }
        else
        {
            return a;
        }
    }

    void ResetTransform(GameObject from, GameObject to)
    {
        from.transform.SetParent(to.transform);
        from.transform.localPosition = new Vector3(to.transform.localPosition.x, 0f, 0f);
        from.transform.localScale = new Vector3(1, 1, 1);        
    }

    public void SetScrollPosition(int rowNumber)
    {
        /* SETUP SCROLL POSITION */
        float rowHeight = rowPrefab.GetComponent<RectTransform>().rect.height;
        float rowToGoTo = rowHeight * rowNumber;
        float gridYPosition = -(((totalAmountRows + 1 - 3.0f) / 2.0f) * rowHeight) + rowToGoTo;

        Vector2 initPos = new Vector2(scrollingGrid.GetComponent<RectTransform>().anchoredPosition.x, scrollingGrid.GetComponent<RectTransform>().anchoredPosition.y - (((totalAmountRows + 1 - 3.0f) / 2.0f) * rowHeight));
        Vector2 desPos = new Vector2(0, gridYPosition + 192);

        StartCoroutine(MoveFromTo(initPos, desPos, 0.5f));
    }

    IEnumerator MoveFromTo(Vector2 pointA, Vector2 pointB, float time)
    {
        float t = 0f;
        Manager_Audio.PlaySound(Manager_Audio.play_scrollMap, gameObject);
        while (t < 1.0f)
        {
            t += Time.deltaTime / time; // Sweeps from 0 to 1 in time seconds
            scrollingGrid.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(pointA, pointB, t); // Set position proportional to t
            Manager_Audio.SendParameterValue(Manager_Audio.adjustScrollPitch, t);
            yield return null;         // Leave the routine and return here in the next frame
        }
        Manager_Audio.PlaySound(Manager_Audio.stop_scrollMap, gameObject);
    }
    #endregion

    private void SaveDict(SaveLevelsToXML e)
    {
        SaveLoadLevels.SaveLevels(nodesInRows);
    }

    public void ResetDatabase()
    {
        DataService dataService = new DataService(StringResources.databaseName);

        dataService.CreateDB(1);
    }
}