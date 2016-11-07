using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour {

    InputScript inputScript;
    CommandsScript commandsScript;
    GameObject lineRenderer;
    List<Vector3> positions;
    public Material LineMaterial;
    bool first = false;
    bool isAdded = false;
    const int zDistance = 89;
    
    void OnEnable()
    {
        positions = new List<Vector3>();
        inputScript = transform.parent.GetComponentInChildren<InputScript>();
        commandsScript = transform.GetComponentInChildren<CommandsScript>();
        lineRenderer = new GameObject();
        lineRenderer.AddComponent<LineRenderer>();
        lineRenderer.name = "Line";
        lineRenderer.GetComponent<LineRenderer>().SetWidth(1f, 1f);
        lineRenderer.transform.SetParent(transform);
        lineRenderer.transform.localPosition = new Vector3(0, 0, 0);
        lineRenderer.transform.localScale = new Vector3(1, 1, 1);
        lineRenderer.GetComponent<LineRenderer>().SetVertexCount(1);
        lineRenderer.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, zDistance));
        lineRenderer.GetComponent<LineRenderer>().material = LineMaterial;
        positions.Add(new Vector3(0, 0, zDistance));

    }

   
	// Update is called once per frame
	void Update () {
      
       
        if (inputScript.buttonClicked)
        {
            
            StartCoroutine(Draw());
        }
	}

    IEnumerator Draw()
    {
        while (Input.GetMouseButton(0))
        {

            //Debug.Log(commandsScript.isOver);
            if (commandsScript.isOver)
            {
                Debug.Log("insiiide");
                //positions.Add(commandsScript.gameObject.transform.position +Vector3.forward*-10);
                //lineRenderer.SetVertexCount(positions.Count);
                //lineRenderer.SetPosition(positions.Count - 1, commandsScript.gameObject.transform.position + Vector3.forward * -10);
                //AddLinePoint(commandsScript.gameObject.transform.position + Vector3.forward * -10);
            }
            else {
                // Debug.Log("insooooo");
                //positions.Add(Input.mousePosition + Vector3.forward * -10);
                //lineRenderer.SetVertexCount(positions.Count);
                //lineRenderer.SetPosition(positions.Count - 1, Input.mousePosition + Vector3.forward * -10);
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 99;

                AddLinePoint(Camera.main.ScreenToWorldPoint(mousePos));
                //RemoveLastLinePoint();
            }

            //lineRenderer.SetVertexCount(positions.Count);
            //lineRenderer.SetPositions(positions.ToArray());
            
            
           // positions.RemoveAt(1);
            
           // Debug.Log(positions.Count);
            
            
            yield return null;
        }
    }

    void AddLinePoint(Vector3 newPoint)
    {
        // Debug.Log(newPoint);

        if (!isAdded)
        {
            positions.Add(newPoint); // add the new point to our saved list of line points
            isAdded = true;
            // Debug.Log(positions.Count);
            lineRenderer.GetComponent<LineRenderer>().SetVertexCount(positions.Count); // set the line’s vertex count to how many points we now have, which will be 1 more than it is currently
            lineRenderer.GetComponent<LineRenderer>().SetPosition(positions.Count - 1, new Vector3(newPoint.x, newPoint.y, newPoint.z)); // add newPoint as the last point on the line (count -1 because the SetPosition is 0-based and Count is 1-based)    
        }
        else
        {
            lineRenderer.GetComponent<LineRenderer>().SetVertexCount(positions.Count); // set the line’s vertex count to how many points we now have, which will be 1 more than it is currently
            lineRenderer.GetComponent<LineRenderer>().SetPosition(positions.Count - 1, new Vector3(newPoint.x, newPoint.y, newPoint.z)); // add newPoint as the last point on the line (count -1 because the SetPosition is 0-based and Count is 1-based)    
        }
    }

    //void RemoveLastLinePoint()
    //{
    //    positions.RemoveAt(positions.Count - 1); // remove the last point from the line
    //    lineRenderer.SetVertexCount(positions.Count); // set the line’s vertex count to how many points we now have, which will be 1 fewer than it is currently       
    //}
}