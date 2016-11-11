using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{

    InputScript inputScript;
    CommandsScript commandsScript;

    public int zDistance = -9;
    bool test = true;
    // Contain current vertex count in line renderer
    public int countVertices = 0;
    public bool notAdded = true;
    public bool centerAdded = false;

    public List<Vector3> drawArray = new List<Vector3>();

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

            if (!centerAdded)
            {
                drawArray.Add(mousePos);
                centerAdded = true;
            }

            if (commandsScript.isOver && notAdded)
            {
                drawArray.Add(mousePos);
                notAdded = false;
                //countVertices++;
                //GetComponent<LineRenderer>().SetVertexCount(countVertices);
                //GetComponent<LineRenderer>().SetPosition(countVertices - 1, commandsScript.gameObject.transform.position);
            }
            else
            {

            }

            if (!commandsScript.isOver)
            {
                notAdded = true;
                // OUTCOMMENT THIS LINE BECAUSE IT IS OVERWRITING THE SNAPPED POINT
                
                //AddLinePoint(Camera.main.ScreenToWorldPoint(mousePos));
            }

            LineRenderer strokeRenderer = GetComponent<LineRenderer>();
            strokeRenderer.SetVertexCount(drawArray.Count+1);
            int i = 0;
            for (i = 0; i < drawArray.Count; i++)
            {
                //Do the conversion here:
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(drawArray[i]);
                strokeRenderer.SetPosition(i, worldPoint);
            }

            strokeRenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(mousePos));
        }

        
    }

    // UPDATE CALLED
    void AddLinePoint(Vector3 newPoint)
    {
        GetComponent<LineRenderer>().SetVertexCount(countVertices);
        GetComponent<LineRenderer>().SetPosition(countVertices - 1, new Vector3(newPoint.x, newPoint.y, newPoint.z));
    }
}