using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class SimpleInputScript : MonoBehaviour
{
    public float timeMax = 0.25f; //Time for command to trigger
    float timer = 0f; // 
    float countdown;
    public bool buttonClicked = false; //if button is clicked
    public GameObject commandPanel;
    SimpleCommandsManager simpleCommandsManager;
    MoveScript moveScript;
    LevelManager levelManager;
    bool front1 = true;
    bool front2 = true;
    bool front3 = true;

    public List<GameObject> buttons;

    void Start()
    {
        levelManager = UnityEngine.Object.FindObjectOfType<LevelManager>();
    }

    public void ButtonDown()
    {
        buttonClicked = true;
        countdown = timeMax;
    }

    public void ButtonUp()
    {
        Time.timeScale = 1f;
        buttonClicked = false;
        if (commandPanel.activeSelf == true)
        {
            simpleCommandsManager = transform.parent.GetComponentInChildren<SimpleCommandsManager>();
            checkForCommand();
            simpleCommandsManager.currentCommand.Clear();
            commandPanel.SetActive(false);
			Manager_Audio.ChangeState (Manager_Audio.commandWheelContainer,Manager_Audio.closeWheel);

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

                    ChangeColor(0);
                    break;
                }
                if (command == simpleCommandsManager.commandsList[1])
                {
                    EventManager.Instance.TriggerEvent(new OffensiveStateEvent());
                    Debug.Log("offensive");

                    ChangeColor(1);
                    break;
                }
                if (command == simpleCommandsManager.commandsList[2])
                {
                    EventManager.Instance.TriggerEvent(new FleeStateEvent());
                    Debug.Log("flee");

                    ChangeColor(2);
                    break;
                }
                if (command == simpleCommandsManager.commandsList[3])
                {
                    EventManager.Instance.TriggerEvent(new FollowStateEvent());
                    Debug.Log("follow");
                    ChangeColor(3);
                    break;
                }
                if (command == simpleCommandsManager.commandsList[4])
                {
                    EventManager.Instance.TriggerEvent(new StayStateEvent());
                    Debug.Log("stay");
                    ChangeColor(4);
                    break;
                }
                if (command == simpleCommandsManager.commandsList[5])
                {
                    EventManager.Instance.TriggerEvent(new ChangeFormationEvent(levelManager.huntersAndPlayer[0]));
					levelManager.huntersAndPlayer[0].GetComponent<HunterStateMachine>().ProjectCommand();
					Debug.Log("hunter1 front/back");                
                    ChangeButtonText(5);
                    ChangeColor(5);

                    break;
                }
                if (command == simpleCommandsManager.commandsList[6])
                {
                    EventManager.Instance.TriggerEvent(new ChangeFormationEvent(levelManager.huntersAndPlayer[1]));
					levelManager.huntersAndPlayer[0].GetComponent<HunterStateMachine>().ProjectCommand();
					Debug.Log("hunter2 front/back");
                    ChangeButtonText(6);
                    ChangeColor(6);
                    break;
                }
                if (command == simpleCommandsManager.commandsList[7])
                {
                    EventManager.Instance.TriggerEvent(new ChangeFormationEvent(levelManager.huntersAndPlayer[2]));
					levelManager.huntersAndPlayer[0].GetComponent<HunterStateMachine>().ProjectCommand();
					Debug.Log("hunter3 front/back");
                    ChangeButtonText(7);
                    ChangeColor(7);
                    
                    break;
                }
            }
        }
    }

    void ChangeColor(int index)
    {
        buttons[index].GetComponent<Image>().color = new Color(0, 0, 0);
        buttons[index].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    void ChangeButtonText(int index)
    {
        Text btnText = buttons[index].transform.GetChild(0).GetComponent<Text>();

        if (btnText.text == "Rear")
            buttons[index].transform.GetChild(0).GetComponent<Text>().text = "Front";
        else
            buttons[index].transform.GetChild(0).GetComponent<Text>().text = "Rear";
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
                Time.timeScale = 0.15f;

                commandPanel.SetActive(true);
				Manager_Audio.PlaySound(Manager_Audio.CommandUI, gameObject);
				Manager_Audio.ChangeState (Manager_Audio.commandWheelContainer,Manager_Audio.openWheel);
            }
        }
    }
}
