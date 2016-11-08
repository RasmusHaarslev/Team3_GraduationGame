using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CommandsScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = true; //button is hovered over
                                
    CommandsManager commandsManager;
    DrawLine drawlineScript;

    void OnEnable()
    {
        commandsManager = commandsManager == null ? transform.parent.GetComponent<CommandsManager>() : commandsManager ;
        drawlineScript = drawlineScript == null ? transform.parent.GetComponent<DrawLine>() : drawlineScript;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

       
        isOver = true;
        Debug.Log(isOver);
        drawlineScript.GetIndexScript(gameObject);
        commandsManager.FillCurrentCommandList(int.Parse(gameObject.name));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        isOver = false;
        Debug.Log("pointerExit");
    }
}