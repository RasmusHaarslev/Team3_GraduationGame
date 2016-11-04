using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public bool isOver = false; //button is hovered over
    public float timeMax = 1f; //time for command to trigger
    float timer = 0f;
    float countdown;
    bool buttonClicked = false; //if button is clicked

    public GameObject commandPanel;

    CommandsManager commandsManager;

    //void test()
    //{
    //    commandsManager = transform.parent.GetComponentInChildren<CommandsManager>();
    //    Debug.Log(commandsManager);
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter");
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        isOver = false;
        buttonClicked = false;
    }

    public void ButtonDown()
    {
        Debug.Log("down");
        buttonClicked = true;
        countdown = timeMax;
    }

    public void ButtonUp()
    {
        //test();
        buttonClicked = false;
        timer = 0;
        if (commandPanel.activeSelf == true)
        {
            commandsManager = transform.parent.GetComponentInChildren<CommandsManager>();
            commandsManager.currentCommand.Clear();
            commandsManager.previousIndex = -1;
            commandPanel.SetActive(false);   
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonClicked)
        {
            timer = Time.deltaTime;
            countdown = countdown - timer;
            if (countdown < 0)
            {
                Debug.Log("do sth");
                commandPanel.SetActive(true);
               // gameObject.SetActive(false);
                
   
            }
        }
    }
}