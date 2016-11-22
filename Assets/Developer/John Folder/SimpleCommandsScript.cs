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
		Manager_Audio.PlaySound(Manager_Audio.HoverCommandUI, gameObject);
        isOver = true;
        if (gameObject.name != "0") { 
            GetComponent<RectTransform>().localScale = new Vector2(1.5f, 1.5f);
            GetComponent<Image>().color = new Color(0, 160, 0);
        }
        simpleCommandsManager.FillCurrentCommandList(int.Parse(gameObject.name));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
        if (gameObject.name != "0")
        {
            GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
            GetComponent<Image>().color = new Color(0, 0, 0);
        }
        if (simpleCommandsManager.currentCommand.Count > 1) { 
        simpleCommandsManager.RemoveCurrentCommandList(1);
        }
    }




}

