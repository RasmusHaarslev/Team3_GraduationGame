using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CommandsManager : MonoBehaviour {

    public List<int> currentCommand = new List<int>();
    public int previousIndex = -1;
    public Text combination;
 
    public void FillCurrentCommandList(int index)
    {

        if (index != previousIndex)
        {
            currentCommand.Add(index);
            combination.text = combination.text + " " + index;
            previousIndex = index;
        }
        
    }
}
