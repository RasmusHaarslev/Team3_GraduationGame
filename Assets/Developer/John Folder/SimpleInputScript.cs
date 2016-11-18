using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SimpleInputScript : MonoBehaviour
{
    public float timeMax = 0.25f; //Time for command to trigger
    float timer = 0f; // 
    float countdown;
    public bool buttonClicked = false; //if button is clicked
    public GameObject commandPanel;
    SimpleCommandsManager simpleCommandsManager;
    MoveScript moveScript;


    public void ButtonDown()
    {
        buttonClicked = true;
        countdown = timeMax;
    }

    public void ButtonUp()
    {

        buttonClicked = false;
        if (commandPanel.activeSelf == true)
        {
            simpleCommandsManager = transform.parent.GetComponentInChildren<SimpleCommandsManager>();
            checkForCommand();
            simpleCommandsManager.currentCommand.Clear();
            commandPanel.SetActive(false);
        } 
    }

    private void checkForCommand()
    {
        foreach (var command in simpleCommandsManager.commandsList)
        {
            if (simpleCommandsManager.currentCommand.SequenceEqual(command))
            {
                if (command == simpleCommandsManager.commandsList[0])
                {
                    Debug.Log("defend");
                    EventManager.Instance.TriggerEvent(new DefendStateEvent());
                    break;
                }
                if (command == simpleCommandsManager.commandsList[1])
                {
                    EventManager.Instance.TriggerEvent(new OffensiveStateEvent());
                    Debug.Log("offensive");
                    break;
                }
                if (command == simpleCommandsManager.commandsList[2])
                {
                    EventManager.Instance.TriggerEvent(new FleeStateEvent());
                    Debug.Log("flee");
                    break;
                }
                if (command == simpleCommandsManager.commandsList[3])
                {
                    EventManager.Instance.TriggerEvent(new FollowStateEvent());
                    Debug.Log("follow");
                    break;
                }
                if (command == simpleCommandsManager.commandsList[4])
                {
                    EventManager.Instance.TriggerEvent(new StayStateEvent());
                    Debug.Log("stay");
                    break;
                }
                if (command == simpleCommandsManager.commandsList[5])
                {
                    EventManager.Instance.TriggerEvent(new ChangeFormationEvent());
                    Debug.Log("front/back");
                    break;
                }
                if (command == simpleCommandsManager.commandsList[6])
                {
                    EventManager.Instance.TriggerEvent(new ChangeFormationEvent());
                    Debug.Log("front/back");
                    break;
                }
                if (command == simpleCommandsManager.commandsList[7])
                {
                    EventManager.Instance.TriggerEvent(new ChangeFormationEvent());
                    Debug.Log("front/back");
                    break;
                }
            }
        }
    }

    void Update()
    {

        if (moveScript == null)
        {
            moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveScript>();
        }
        if (commandPanel.activeSelf == true)
        {
            moveScript.movement = false;
        }
        else
        {
            moveScript.movement = true;
        }
        if (buttonClicked && commandPanel.activeSelf == false)
        {

            countdown = countdown - Time.deltaTime;

            if (countdown < 0)
            {
                commandPanel.SetActive(true);        
            }
        }
    }
}
