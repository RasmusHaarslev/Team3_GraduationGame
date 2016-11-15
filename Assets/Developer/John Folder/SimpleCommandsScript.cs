using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class SimpleCommandsScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = true; //button is hovered over

    SimpleCommandsManager simpleCommandsManager;
  
    void OnEnable()
    {
        simpleCommandsManager = simpleCommandsManager == null ? transform.parent.GetComponent<SimpleCommandsManager>() : simpleCommandsManager;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {

        isOver = true;  
        simpleCommandsManager.FillCurrentCommandList(int.Parse(gameObject.name));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
        if (simpleCommandsManager.currentCommand.Count > 1) { 
        simpleCommandsManager.RemoveCurrentCommandList(1);
        }
    }




}

