using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class SimpleCommandsManager : MonoBehaviour
{
    
    public List<int> currentCommand = new List<int>(); //List<int> currentCommand = new List<int>();
    public int previousIndex = -1; //int previousIndex = -1;
    public  List<List<int>> commandsList = new List<List<int>>();
    public Button button;
    bool clickUp;

    void Start()
    {
        commandsList.Add(new List<int> { 0, 1 }); // DefendStateEvent
        commandsList.Add(new List<int> { 0, 2 }); // OffensiveStateEvent
        commandsList.Add(new List<int> { 0, 3 }); // FleeStateEvent
        commandsList.Add(new List<int> { 0, 4 }); // FollowStateEvent
        commandsList.Add(new List<int> { 0, 5 }); // StayStateEvent
        commandsList.Add(new List<int> { 0, 6 }); // Hunter1ToggleEvent
        commandsList.Add(new List<int> { 0, 7 }); // Hunter2ToggleEvent
        commandsList.Add(new List<int> { 0, 8 }); // Hunter3ToggleEvent

    }

    public void FillCurrentCommandList(int index)
    {
        currentCommand.Add(index);
        
    }

    public void RemoveCurrentCommandList(int index)
    {
        currentCommand.RemoveAt(index);
        
    }
}

