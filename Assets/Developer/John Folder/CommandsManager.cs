using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandsManager : MonoBehaviour {

    public List<int> currentCommand = new List<int>(); //List<int> currentCommand = new List<int>();
    public int previousIndex = -1; //int previousIndex = -1;

    //InputScript inputScript;

    //void Start()
    //{
    //    inputScript = transform.parent.GetComponentInChildren<InputScript>();
    //}
   
    public void FillCurrentCommandList(int index)
    {

        if (index != previousIndex)
        {
            currentCommand.Add(index);
            foreach (var i in currentCommand)
            {
                Debug.Log(i);
            }
            previousIndex = index;
        }
        
    }
}
