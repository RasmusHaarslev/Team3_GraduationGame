using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class CommandsScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = true; //button is hovered over

    CommandsManager commandsManager;
    DrawLine drawlineScript;
   

    void OnEnable()
    {

        commandsManager = commandsManager == null ? transform.parent.GetComponent<CommandsManager>() : commandsManager;
        drawlineScript = drawlineScript == null ? transform.parent.GetComponent<DrawLine>() : drawlineScript;
        
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
       
        isOver = true;
        drawlineScript.GetIndexScript(gameObject);
        commandsManager.FillCurrentCommandList(int.Parse(gameObject.name));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }

   

   
}

