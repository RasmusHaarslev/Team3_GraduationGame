using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputScript : MonoBehaviour
{
    public float timeMax = 0.25f; //Time for command to trigger
    float timer = 0f; // 
    float countdown;
    public bool buttonClicked = false; //if button is clicked

    public GameObject commandPanel;

    CommandsManager commandsManager;

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
            commandsManager = transform.parent.GetComponentInChildren<CommandsManager>();
            commandsManager.currentCommand.Clear();
            commandsManager.previousIndex = -1;

            commandPanel.GetComponent<LineRenderer>().SetVertexCount(0);
            commandPanel.GetComponent<DrawLine>().countVertices = 0;

            commandPanel.SetActive(false);
        }

    }

    void Update()
    {
        if (buttonClicked && commandPanel.activeSelf == false)
        {
            countdown = countdown - Time.deltaTime;

            if (countdown < 0)
            {
                commandPanel.SetActive(true);
                commandPanel.GetComponent<LineRenderer>().SetVertexCount(1);
                commandPanel.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, commandPanel.GetComponent<DrawLine>().zDistance));
                commandPanel.GetComponent<DrawLine>().countVertices++;
            }
        }
    }
}