using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

    public GameObject selectedObject;
    public GameObject hoveredObject;

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.transform.gameObject;
            Debug.Log("mouse is over: " + hitObject.name);
            Debug.Log("collider is: " + hitInfo.collider.name);
            SelectObject(hitObject);
        }
        else
        {
            ClearSelection();
        }

        if (Input.GetMouseButtonDown(0))
        {
            selectedObject = hoveredObject;
            Debug.Log("selected object is: "+ selectedObject);
        }
	}

    void SelectObject(GameObject obj)
    {
        if(selectedObject != null)
        {
            if(obj == selectedObject)
            {
                return;
            }

            ClearSelection();
        }

        hoveredObject = obj;
    }

    void ClearSelection()
    {
        selectedObject = null;
    }
}
