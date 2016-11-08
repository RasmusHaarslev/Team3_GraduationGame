using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectionGenerator : MonoBehaviour
{

    public GameObject scrollRect;
    public GameObject scrollingGrid;
    public GameObject dropDown;

    public GameObject rowPrefab;
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

    int numOfLastLevels = 1;
    int totalAmountRows;
    int nodeCounter = 0;

    void Awake()
    {
        InstantiateRows(amountOfRows - 1);

        SetScrollPosition(0);
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

    /// <summary>
    /// Building the Rows and randomly selection how many nodes pr. row
    /// </summary>
    /// <param name="rowAmount"></param>
    public void InstantiateRows(int rowAmount)
    {

        for (int i = 0; i <= rowAmount; i++)
        {

            if (i == 0)
            {
                SetupRootNode();
                continue;
            }
            totalAmountRows += 1;

            GameObject row = Instantiate(rowPrefab);
            ResetTransform(row, scrollingGrid);
            row.name = (totalAmountRows).ToString();

            numOfLastLevels = randomController(numOfLastLevels);

            float startX = 0f;
            float increaseX = 0f;

            switch (numOfLastLevels)
            {
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

            for (int j = 0; j < numOfLastLevels; j++)
            {
                nodeCounter++;

                string levelName = (totalAmountRows) + "." + j;
                GameObject newNode = Instantiate(nodePrefab);
                ResetTransform(newNode, row);

                SetupValuesInNode(newNode);

                newNode.name = levelName;
                newNode.transform.localPosition = new Vector3(startX, 0, 0);

                newNode.GetComponent<Node>().OnCreate(nodeCounter);

                startX += increaseX;
            }
        }
        initialiseDropDown();
    }

    void SetupRootNode()
    {

        GameObject row = Instantiate(rowPrefab);
        ResetTransform(row, scrollingGrid);
        row.name = "0";

        GameObject rootNode = Instantiate(nodePrefab);
        ResetTransform(rootNode, row);

        SetupValuesInNode(rootNode);
        rootNode.name = "0.0";
    }

    void SetupValuesInNode(GameObject node)
    {
        node.GetComponent<Node>().Level = totalAmountRows;
        node.GetComponent<Node>().NodeId = nodeCounter;
        node.GetComponent<Node>().sceneSelection = Random.Range(2, numberOfScenes + 2);
        node.GetComponent<Node>().itemDropAmount = Random.Range(1, itemDropAmount);
        node.GetComponent<Node>().probabilityWolves = probabilityWolves;
        node.GetComponent<Node>().probabilityTribes = probabilityTribes;
        node.GetComponent<Node>().probabilityChoice = probabilityChoices;
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

        from.transform.localPosition = new Vector3(to.transform.position.x, to.transform.position.y, 0f);
        from.transform.localScale = new Vector3(1, 1, 1);
    }

    #region Helper function
    /// <summary>
    /// Button click (Temporary Functionality)
    /// </summary>
    /// <param name="amount">Will insert the number of rows</param>
    public void AddItemToGrid(int amount)
    {
        InstantiateRows(amount);
    }

    #endregion
}
