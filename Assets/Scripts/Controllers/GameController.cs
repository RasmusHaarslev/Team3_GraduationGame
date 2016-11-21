﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public int _FOOD = 10;
    public int _VILLAGERS = 10;
    public int _SCRAPS = 10;
    public int _PREMIUM = 10;

    #region Setup Instance
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameController");
                _instance = go.AddComponent<GameController>();
            }
            return _instance;
        }
    }
    #endregion

    void OnEnable()
    {
        EventManager.Instance.StartListening<ChangeResources>(UpdateResources);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<ChangeResources>(UpdateResources);
    }

    public void UpdateResources(ChangeResources e)
    {
        _FOOD += e.food;
        _VILLAGERS += e.villager;
        _SCRAPS += e.scraps;
        _PREMIUM += e.premium;
        EventManager.Instance.TriggerEvent(new ResourcesUpdated());
    }

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string scene)
    {
        if (SceneTransistion.instance != null)
        {
            SceneTransistion.instance.LoadScene(scene);
        }
        else
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

    public void ToggleSound()
    {
        GameController.Instance.GetComponent<AudioSource>().volume = GameController.Instance.GetComponent<AudioSource>().volume == 1 ? 0 : 1;
    }

    public void AdjustSound(float value)
    {
        GameController.Instance.GetComponent<AudioSource>().volume = value;
    }

    public void LoadLevel()
    {
        var sceneDirectory = Directory.CreateDirectory("Assets/_Scenes/Levels");
        List<string> scenes = new List<string>();

        foreach (var scene in sceneDirectory.GetFiles())
        {
            if (scene.Name.EndsWith(".unity"))
            {
                scenes.Add(scene.Name.Split('.')[0]);
            }
        }

        var randomScene = scenes[UnityEngine.Random.Range(0,scenes.Count-1)];
        SceneManager.LoadScene(randomScene);
    }
}
