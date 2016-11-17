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
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
    }
}