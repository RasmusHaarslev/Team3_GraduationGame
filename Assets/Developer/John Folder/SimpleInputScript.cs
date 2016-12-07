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
	LevelGenerator levelGenerator;
	bool front1 = true;
	bool front2 = true;
	bool front3 = true;

	public List<GameObject> buttons;

	void Start()
	{
		Manager_Audio.ChangeState(Manager_Audio.commandWheelContainer, Manager_Audio.closeWheel);
		levelManager = UnityEngine.Object.FindObjectOfType<LevelManager>();
		levelGenerator = UnityEngine.Object.FindObjectOfType<LevelGenerator>();
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
			Manager_Audio.ChangeState(Manager_Audio.commandWheelContainer, Manager_Audio.closeWheel);

		}
	}

	private void checkForCommand()
	{
		foreach (var command in simpleCommandsManager.commandsList)
		{
			if (simpleCommandsManager.currentCommand.SequenceEqual(command))
			{
				if (command == simpleCommandsManager.commandsList[0] && !simpleCommandsManager.inDefenseState)
				{
					//Debug.Log("defend");
					EventManager.Instance.TriggerEvent(new DefendStateEvent());
					EventManager.Instance.TriggerEvent(new CommandEvent());
					simpleCommandsManager.inDefenseState = true;
					simpleCommandsManager.currentCommandBtnText.text = TranslationManager.Instance.GetTranslation("Offensive");

                    ChangeColor(0);
					break;
				}
				if (command == simpleCommandsManager.commandsList[0] && simpleCommandsManager.inDefenseState)
				{
					EventManager.Instance.TriggerEvent(new OffensiveStateEvent());
					EventManager.Instance.TriggerEvent(new CommandEvent());
					//Debug.Log("offensive");
					simpleCommandsManager.inDefenseState = false;
					simpleCommandsManager.currentCommandBtnText.text = TranslationManager.Instance.GetTranslation("Defensive"); 
					ChangeColor(0);
					break;
				}
				if (command == simpleCommandsManager.commandsList[2])
				{
					EventManager.Instance.TriggerEvent(new FleeStateEvent());
					EventManager.Instance.TriggerEvent(new CommandEvent());

                    simpleCommandsManager.currentCommandBtnText.text = TranslationManager.Instance.GetTranslation("Flee");

                    ChangeColor(2);
					break;
				}
				if (command == simpleCommandsManager.commandsList[4] && !simpleCommandsManager.inFollowState)
				{
					EventManager.Instance.TriggerEvent(new FollowStateEvent());
					EventManager.Instance.TriggerEvent(new CommandEvent());

					simpleCommandsManager.inFollowState = true;
					simpleCommandsManager.currentCommandBtnText.text = TranslationManager.Instance.GetTranslation("Stay");
					ChangeColor(4);
					break;
				}
				if (command == simpleCommandsManager.commandsList[4] && simpleCommandsManager.inFollowState)
				{
					EventManager.Instance.TriggerEvent(new StayStateEvent());
					EventManager.Instance.TriggerEvent(new CommandEvent());

					simpleCommandsManager.inFollowState = false;
					simpleCommandsManager.currentCommandBtnText.text = TranslationManager.Instance.GetTranslation("Follow");
					ChangeColor(4);
					break;
				}
				if (command == simpleCommandsManager.commandsList[5])
				{
					if (levelManager.huntersAndPlayer.Count >= 2)
					{
						EventManager.Instance.TriggerEvent(new ChangeFormationEvent(levelManager.huntersAndPlayer[0]));
						levelManager.huntersAndPlayer[0].GetComponent<HunterStateMachine>().ProjectCommand();
					}
					Debug.Log("hunter1 front/back");
					ChangeButtonText(5);
					ChangeColor(5);

					break;
				}
				if (command == simpleCommandsManager.commandsList[6])
				{
					if (levelManager.huntersAndPlayer.Count >= 3)
					{
						EventManager.Instance.TriggerEvent(new ChangeFormationEvent(levelManager.huntersAndPlayer[1]));
						levelManager.huntersAndPlayer[1].GetComponent<HunterStateMachine>().ProjectCommand();
					}
					Debug.Log("hunter2 front/back");
					ChangeButtonText(6);
					ChangeColor(6);
					break;
				}
				if (command == simpleCommandsManager.commandsList[7])
				{
					if (levelManager.huntersAndPlayer.Count >= 4)
					{
						EventManager.Instance.TriggerEvent(new ChangeFormationEvent(levelManager.huntersAndPlayer[2]));
						levelManager.huntersAndPlayer[2].GetComponent<HunterStateMachine>().ProjectCommand();
					}
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
		buttons[index].GetComponent<Image>().color = new Color(255, 255, 255);
		buttons[index].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
	}

	void ChangeButtonText(int index)
	{
		Text btnText = buttons[index].transform.GetChild(0).GetComponent<Text>();

		if (btnText.text == "Rear")
			buttons[index].transform.GetChild(0).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Front");
		else
			buttons[index].transform.GetChild(0).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Rear");
	}

	void Update()
	{
		if (!levelGenerator.isTutorial)
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
		}

		if (buttonClicked && commandPanel.activeSelf == false)
		{

			countdown = countdown - Time.deltaTime;

			if (countdown < 0)
			{
				Time.timeScale = 0.15f;

				commandPanel.SetActive(true);
				Manager_Audio.PlaySound(Manager_Audio.CommandUI, gameObject);
				Manager_Audio.ChangeState(Manager_Audio.commandWheelContainer, Manager_Audio.openWheel);
			}
		}
	}
}
