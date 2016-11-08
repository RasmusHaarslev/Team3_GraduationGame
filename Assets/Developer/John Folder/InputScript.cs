using System;
using UnityEngine;
using UnityEngine.EventSystems;

//public class InputScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
public class InputScript : MonoBehaviour
{
    //public bool isOver = false; //button is hovered over
    public float timeMax = 0.25f; //time for command to trigger
    float timer = 0f;
    float countdown;
    public bool buttonClicked = false; //if button is clicked

    public GameObject commandPanel;

    CommandsManager commandsManager;

    //void test()
    //{
    //    commandsManager = transform.parent.GetComponentInChildren<CommandsManager>();
    //    Debug.Log(commandsManager);
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Debug.Log("Mouse enter");
    //    isOver = true;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    Debug.Log("Mouse exit");
    //    isOver = false;
    //    //buttonClicked = false;
    //    Debug.Log("change onpinterexit");
    //}

    public void ButtonDown()
    {
        buttonClicked = true;
        countdown = timeMax;
    }

    public void ButtonUp()
    {
      
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
        if (buttonClicked && commandPanel.activeSelf == false)
        {
            timer = Time.deltaTime;
            countdown = countdown - timer;
            if (countdown < 0 )
            {
                Debug.Log("do sth");
                commandPanel.SetActive(true);
               // gameObject.SetActive(false);
                
   
            }
        }
    }
}