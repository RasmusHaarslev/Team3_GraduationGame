using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CutsceneHelper : MonoBehaviour
{
    public bool ToCamp;

    void Start()
    {
        Invoke("LoadScene", 6f);
    }

    public void LoadScene()
    {
        if (!ToCamp)
        {
            GameController.Instance.LoadLevel();
        }
        else
        { 
            GameController.Instance.LoadScene("CampManagement");
        }
    }
}