using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{

    public GameObject selectedObject;
    public GameObject hoveredObject;
    private GameObject currentActivePanel = null;
    public List<GameObject> panelList = new List<GameObject>(); 
    public bool activePanel = false;
    public GameObject BackPanel;

    // Update is called once per frame
    void Update () {


        
        if (Input.GetMouseButtonDown(0) && !activePanel)
        {

            CheckTarget();
            ActivatePanel();
            //HideIfClickedOutside(panelList[0]);
        }
        
	}

    void CheckTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.transform.gameObject;

            SelectObject(hitObject);
        }
        else
        {
            ClearSelection();
        }
    }

    void ActivatePanel()
    {
        selectedObject = hoveredObject;
        //Debug.Log("selected object is: " + selectedObject);
        if (selectedObject.CompareTag("Friendly") || (selectedObject.CompareTag("Player")))
        {
            //ActivatePanel(panelList[2]);
            panelList[2].SetActive(true);
            activePanel = true;
        }
        if (selectedObject.CompareTag("Tent"))
        {
            // ActivatePanel(panelList[0]);
            panelList[0].SetActive(true);
            activePanel = true;
        }
    }

    //void ActivatePanel(GameObject panelToActivate)
    //{
    //    if(panelToActivate != currentActivePanel)
    //    {
    //        DeactivateCurrentActivePanel();
    //        panelToActivate.SetActive(true);
    //        currentActivePanel = panelToActivate;
    //        activePanel = true;
    //    }
    //    else
    //    { }
        
    //}

    //void DeactivateCurrentActivePanel()
    //{
    //    foreach(GameObject panel in panelList)
    //    {        
    //        if(panel.activeSelf == true)
    //        {          
    //            panel.SetActive(false);            
    //        }
    //    }
    //    activePanel = false;
    //}

 

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

    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                panel.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            panel.SetActive(false);
        }
    }

}
