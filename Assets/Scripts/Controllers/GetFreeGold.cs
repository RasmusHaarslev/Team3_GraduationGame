using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GetFreeGold : MonoBehaviour
{
    public void Get100FreeGold()
    {
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 100);
    }
}