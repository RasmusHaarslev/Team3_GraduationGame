using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

public class EquippableItem : MonoBehaviour
{


    //values gained from the database
    public EquippableitemValues itemValues;
   

    /// <summary>
    /// Set the equippable item values passed in the parameter
    /// </summary>
    /// <param name="initValues"></param>
    public void init(EquippableitemValues initValues)
    {
        itemValues = initValues;
       
    }


}

