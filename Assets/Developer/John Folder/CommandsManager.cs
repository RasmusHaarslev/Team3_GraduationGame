using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class CommandsManager : MonoBehaviour
{
    

    public List<int> currentCommand = new List<int>(); //List<int> currentCommand = new List<int>();
    public int previousIndex = -1; //int previousIndex = -1;
    List<List<int>> commandsList = new List<List<int>>();
    public Button button;

    void Start()
    {
        commandsList.Add(new List<int> { 4, 1, 2 }); // DefendStateEvent
        commandsList.Add(new List<int> { 4, 3, 6 }); // OffensiveStateEvent
        commandsList.Add(new List<int> { 4, 3, 5 });

    }

    public void FillCurrentCommandList(int index)
    {

        if (index != previousIndex)
        {
            currentCommand.Add(index);
            foreach (var command in commandsList)
            {
                if (currentCommand.SequenceEqual(command))
                {
                    foreach (var i in command)
                    {
                        Debug.Log(i);
                    }
                    if (command == commandsList[0])
                    {
                        Debug.Log("defend");
                        //EventManager.Instance.TriggerEvent(new DefendStateEvent());
                        button.GetComponent<InputScript>().ButtonUp();
                        break;
                    }
                    if (command == commandsList[1])
                    {
                        EventManager.Instance.TriggerEvent(new OffensiveStateEvent());
                        Debug.Log("offensive");
                        button.GetComponent<InputScript>().ButtonUp();
                        break;
                    }
                  
                    
                }
            }
            
            previousIndex = index;
        }

    }
}
