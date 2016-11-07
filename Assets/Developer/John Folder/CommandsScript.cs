using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CommandsScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false; //button is hovered over
                                
    CommandsManager commandsManager;

    void OnEnable()
    {
        commandsManager = commandsManager == null ? transform.parent.GetComponent<CommandsManager>() : commandsManager ;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        Debug.Log("Mouse enter");
        isOver = true;
        Debug.Log(isOver);
        commandsManager.FillCurrentCommandList(int.Parse(gameObject.name));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        isOver = false;
        Debug.Log("pointerExit");
    }
}