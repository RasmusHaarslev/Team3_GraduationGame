using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour {

    InputScript inputScript;
    CommandsScript commandsScript;
    GameObject lineRenderer;
    List<Vector3> positions;
    //Vector3[] positions = new Vector3[10];
    public Material LineMaterial;
 
    bool isAdded = false;
    const int zDistance = 89;
    // int i = 0;

    bool normal = true;
    void Start()
    {
        positions = new List<Vector3>();
        inputScript = transform.parent.GetComponentInChildren<InputScript>();
       
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
            Draw();
           // StartCoroutine(Draw());
        }
	}

    //IEnumerator Draw()
    private void Draw()
    {
        if (Input.GetMouseButton(0))
        {


            if (normal)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 99;

                AddLinePoint(Camera.main.ScreenToWorldPoint(mousePos));
            }
           


            if (commandsScript.isOver)
            {
                AddLinePoint(commandsScript.gameObject.transform.position);
                isAdded = false;
            }


        }
    }

    public  void GetIndexScript(GameObject gameObject)
    {
        foreach (Transform child in transform)
        {
        
            if(child.name == gameObject.name)
            {
                Debug.Log(child.name);
                commandsScript = gameObject.transform.GetComponentInChildren<CommandsScript>();
            }
            
        }
        
    }

    void AddLinePoint(Vector3 newPoint)
    {


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

}