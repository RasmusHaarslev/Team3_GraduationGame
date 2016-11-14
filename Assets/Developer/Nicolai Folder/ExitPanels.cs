using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ExitPanels : MonoBehaviour,IPointerDownHandler
{
    MouseManager mouseManagerScript;
    public RectTransform[] children;

    void Start()
    {
        mouseManagerScript = GetComponentInParent<MouseManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (mouseManagerScript.activePanel)
        {
            
            foreach (var child in children)
            {
                child.gameObject.SetActive(false);
                Debug.Log("clicked on back panel");
            }
            mouseManagerScript.activePanel = false;
        }
        
    }
}