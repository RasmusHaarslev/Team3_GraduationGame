using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{

    InputScript inputScript;
    CommandsScript commandsScript;

    bool isAdded = false;
    public int zDistance = -9;
    bool test = true;
    // Contain current vertex count in line renderer
    public int countVertices = 0;
    bool notAdded = true;

    void OnEnable()
    {
        inputScript = transform.parent.GetComponentInChildren<InputScript>();
    }


    void Update()
    {
        if (inputScript.buttonClicked)
        {
            Draw();
        }
    }

    public void GetIndexScript(GameObject gameObject)
    {
        foreach (Transform child in transform)
        {

            if (child.name == gameObject.name)
            {
                commandsScript = gameObject.transform.GetComponentInChildren<CommandsScript>();
            }

        }
    }

    //USED IN UPDATE (IF CLICKED)
    private void Draw()
    {
        if (Input.GetMouseButton(0))
        {

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1;

            if (commandsScript.isOver && notAdded)
            {
                notAdded = false;
                countVertices++;
                GetComponent<LineRenderer>().SetVertexCount(countVertices);
                GetComponent<LineRenderer>().SetPosition(countVertices - 1, commandsScript.gameObject.transform.position);
            }
            else
            {

            }

            if (!commandsScript.isOver)
            {
                notAdded = true;
                // OUTCOMMENT THIS LINE BECAUSE IT IS OVERWRITING THE SNAPPED POINT
                AddLinePoint(Camera.main.ScreenToWorldPoint(mousePos));
            }
        }
    }

    // UPDATE CALLED
    void AddLinePoint(Vector3 newPoint)
    {
        GetComponent<LineRenderer>().SetVertexCount(countVertices);
        GetComponent<LineRenderer>().SetPosition(countVertices - 1, new Vector3(newPoint.x, newPoint.y, newPoint.z));
    }
}