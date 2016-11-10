﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectionGenerator : MonoBehaviour {

    #region VARIABLES
    public GameObject scrollRect;
    public GameObject scrollingGrid;
    public GameObject dropDown;

    public GameObject rowPrefab;
    public GameObject rowImagePrefab;
    public GameObject nodePrefab;

    public int amountOfRows = 0;

    [Tooltip("The distribution of wolve camps as a percentage. (If x in wolves and 0 in tribes and choices, there will only spawn wolve camps")]
    [Range(0, 6)]
    public int probabilityWolves;
    [Tooltip("The distribution of tribe camps as a percentage. (If x in wolves and 0 in tribes and choices, there will only spawn wolve camps")]
    [Range(0, 6)]
    public int probabilityTribes;
    [Tooltip("The distribution of choice camps as a percentage. (If x in wolves and 0 in tribes and choices, there will only spawn wolve camps")]
    [Range(0, 6)]
    public int probabilityChoices;

    [Tooltip("Determine the max amonut of items that drop within a single level")]
    [Range(0, 55)]
    public int itemDropAmount;

    [Tooltip("This should always contain the number of different levels we have")]
    public int numberOfScenes;

    [Tooltip("Lowest amount of food to use to go to a level")]
    [Range(0, 55)]
    public int LowestTravelCost;

    [Tooltip("Highest amount of food to use to go to a level")]
    [Range(0, 55)]
    public int HighestTravelCost;

    int numOfLastLevels = 1;
    int totalAmountRows;
    int nodeCounter = 0;
    #endregion

    Dictionary<int, List<GameObject>> nodesInRows = new Dictionary<int, List<GameObject>>();

    void Awake()
    {
        InstantiateRows(amountOfRows);
        SetScrollPosition(0);
    }

    /// <summary>
    /// Building the Rows and randomly selection how many nodes pr. row
    /// </summary>
    /// <param name="rowAmount"></param>
    public void InstantiateRows(int rowAmount) {

        for (int i = 0; i <= rowAmount; i++) {

            GameObject row = Instantiate(rowPrefab);
            ResetTransform(row, scrollingGrid);
            row.name = (totalAmountRows).ToString();

            int numOfParents = numOfLastLevels;

            if ( i != 0) { 
                numOfLastLevels = randomController(numOfLastLevels);
            } else
            {
                numOfLastLevels = 1;
            }

            float startX = 0f;
            float increaseX = 0f;

            switch (numOfLastLevels) {
                case 1:
                    startX = 0;
                    increaseX = 0;
                    break;
                case 2:
                    startX = -241.5f;
                    increaseX = 483f;
                    break;
                case 3:
                    startX = -483f;
                    increaseX = 483f;
                    break;
                case 4:
                    startX = -724f;
                    increaseX = 482f;
                    break;
            }

            List<GameObject> rowNodes = new List<GameObject>();
            Debug.Log("THIS LEVEL COUNT : " + numOfLastLevels);

            for (int j = 0; j < numOfLastLevels; j++) {
                nodeCounter++;
                
                GameObject newNode = Instantiate(nodePrefab);
                ResetTransform(newNode, row);

                SetupValuesInNode(newNode);

                newNode.name = (totalAmountRows) + "." + j;
                newNode.transform.localPosition = new Vector3(startX, newNode.transform.localPosition.y - 84f, 0);

                newNode.GetComponent<Node>().OnCreate(nodeCounter);

                rowNodes.Add(newNode);

                if (nodeCounter > 1) {
                    int counter = 0;
                    foreach (var node in nodesInRows[totalAmountRows - 1]) {
                        if (numOfParents == 1 || numOfLastLevels == 1) { 
                            node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                        } else if (numOfParents == 4)
                        {
                            switch (numOfLastLevels)
                            {
                                case 3:
                                    if(counter == 0 && j == 0) { 
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    } else if (counter == 1 && j < 2)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    } else if (counter == 2 && j > 0 && j < 3)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    } else if (counter == 3 && j == 2)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    break;
                                case 2:
                                    if (counter < 2 && j == 0)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    else if (counter > 1 && j == 1)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    break;
                            }
                        }
                        else if (numOfParents == 3)
                        {
                            switch (numOfLastLevels)
                            {
                                case 4:
                                    if (counter == 0 && j < 2)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    else if (counter == 1 && j > 0 && j < 3)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    else if (counter == 2 && j > 1 && j < 4)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    break;
                                case 2:
                                    if (counter == 0 && j == 0)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    else if (counter == 1)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    } else if (counter == 2 && j > 0)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    break;
                            }
                        }
                        else if (numOfParents == 2)
                        {
                            switch (numOfLastLevels)
                            {
                                case 4:
                                    if (counter == 0 && j < 2)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    else if (counter == 1 && j > 1 && j < 4)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    break;
                                case 3:
                                    if (counter == 0 && j < 2)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    else if (counter == 1 && j > 1)
                                    {
                                        node.GetComponent<Node>().AddLink(newNode, HighestTravelCost);
                                    }
                                    break;
                            }
                        }

                        counter++;
                    }
                }

                startX += increaseX;
            }
            
            nodesInRows.Add(totalAmountRows, rowNodes);

            if (nodeCounter > 1) { 
                GameObject imageRow = Instantiate(rowImagePrefab);
                ResetTransform(imageRow, row);
                imageRow.transform.localPosition = new Vector3(imageRow.transform.position.x, 100f, 0);
                imageRow.GetComponent<AddImageRow>().InsertImage(numOfLastLevels, numOfParents);
            } 

            totalAmountRows += 1;
        }

        initialiseDropDown();
        //printDict();
    }

    void SetupValuesInNode(GameObject node)
    {
        node.GetComponent<Node>().Level = totalAmountRows;
        node.GetComponent<Node>().TravelCost = Random.Range(LowestTravelCost, HighestTravelCost); ;
        node.GetComponent<Node>().sceneSelection = Random.Range(2, numberOfScenes + 2);
        node.GetComponent<Node>().itemDropAmount = Random.Range(1, itemDropAmount);
        node.GetComponent<Node>().probabilityWolves = probabilityWolves;
        node.GetComponent<Node>().probabilityTribes = probabilityTribes;
        node.GetComponent<Node>().probabilityChoice = probabilityChoices;
    }

    #region Helper function

    void printDict(int startFromRow)
    {
        for (int key = startFromRow; key < nodesInRows.Count; key++)
        {

        }
        foreach (KeyValuePair<int, List<GameObject>> kvp in nodesInRows)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value.Count));
        }
    }

    /// <summary>
    /// Return a new random integer between 1 and 4 different from the last
    /// </summary>
    int randomController(int oldNumber)
    {
        int a = Random.Range(1, 5);

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

    /// <summary>
    /// Button click (Temporary Functionality)
    /// </summary>
    /// <param name="amount">Will insert the number of rows</param>
    public void AddItemToGrid(int amount)
    {
        InstantiateRows(amount);
    }

    void initialiseDropDown()
    {
        var dropdown = dropDown.GetComponent<Dropdown>();
        dropdown.options.Clear();
        for (int i = 0; i <= totalAmountRows - 2; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData(i.ToString()));
        }

        if (totalAmountRows + 1 == amountOfRows)
        {
            dropdown.onValueChanged.AddListener(delegate {
                myDropdownValueChangedHandler(dropdown);
            });
        }
    }

    private void myDropdownValueChangedHandler(Dropdown target)
    {
        SetScrollPosition(target.value);
    }

    void SetScrollPosition(int rowNumber)
    {
        /* SETUP SCROLL POSITION */
        float rowHeight = rowPrefab.GetComponent<RectTransform>().rect.height;
        float rowToGoTo = rowHeight * rowNumber;
        float gridYPosition = -(((totalAmountRows + 1 - 3.0f) / 2.0f) * rowHeight) + rowToGoTo;

        scrollingGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, gridYPosition);
    }
    #endregion
}