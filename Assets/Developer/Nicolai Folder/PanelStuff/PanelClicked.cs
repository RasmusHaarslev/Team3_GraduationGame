using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PanelClicked : MonoBehaviour, IPointerClickHandler
{

    public bool isClicked;

    void OnEnable() {
        isClicked = true;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
      
    }
}