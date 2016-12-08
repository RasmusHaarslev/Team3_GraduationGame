using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelaySceneLoad: MonoBehaviour
{
    public float TimeDelay;
    public string SceneToLoad;

    void Awake()
    {
        Invoke("LoadScene",TimeDelay);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}