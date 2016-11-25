﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    public int InitialFood = 10;
    public int InitialVillages = 10;
    public int InitialScrap = 10;
    public int InitialPremium = 10;

    public int _FOOD = 10;
    public int _VILLAGERS = 10;
    public int _SCRAPS = 10;
    public int _PREMIUM = 10;

    [HideInInspector]
    public DataService _dataService;

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
        if (_FOOD + e.food < 0)
        {
            e.villager = e.villager - (Mathf.Abs(e.food) - _FOOD);
            e.food = -_FOOD;
        }

        _FOOD = (_FOOD + e.food < 0) ? 0 : _FOOD + e.food;
        _VILLAGERS = (_VILLAGERS + e.villager < 0) ? 0 : _VILLAGERS + e.villager;
        _SCRAPS = (_SCRAPS + e.scraps < 0) ? 0 : _SCRAPS + e.scraps;
        _PREMIUM = (_PREMIUM + e.premium < 0) ? 0 : _PREMIUM + e.premium;
        EventManager.Instance.TriggerEvent(new ResourcesUpdated());
        SaveResources();
    }

    private void SaveResources()
    {
        PlayerPrefs.SetInt("Food", _FOOD);
        PlayerPrefs.SetInt("Villagers", _VILLAGERS);
        PlayerPrefs.SetInt("Scraps", _SCRAPS);
        PlayerPrefs.SetInt("Premium", _PREMIUM);
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

        _dataService = new DataService(StringResources.databaseName);

        if (PlayerPrefs.HasKey("Food"))
        {
            _FOOD = PlayerPrefs.GetInt("Food");
            _VILLAGERS = PlayerPrefs.GetInt("Villagers");
            _SCRAPS = PlayerPrefs.GetInt("Scraps");
            _PREMIUM = PlayerPrefs.GetInt("Premium");
        }
        else
        {
            PlayerPrefs.SetInt("Food", InitialFood);
            PlayerPrefs.SetInt("Villagers", InitialVillages);
            PlayerPrefs.SetInt("Scraps", InitialScrap);
            PlayerPrefs.SetInt("Premium", InitialPremium);
        }
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
        var sceneListTxt = Resources.Load("ScenesList", typeof(TextAsset)) as TextAsset;

        System.IO.StringReader reader = new System.IO.StringReader(sceneListTxt.text);
        List<string> scenes = new List<string>();

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            scenes.Add(line);
        }

        var randomScene = scenes[UnityEngine.Random.Range(0, scenes.Count - 1)];
        SceneManager.LoadScene(randomScene);
    }

    public void LoseGame()
    {
        PlayerPrefs.DeleteAll();
        ResetResources();
        DataService dataService = new DataService(StringResources.databaseName);
        dataService.ResetDatabase();
    }

    public void ResetResources()
    {
        _FOOD = InitialFood;
        _VILLAGERS = InitialVillages;
        _SCRAPS = InitialScrap;
        _PREMIUM = InitialPremium;

        PlayerPrefs.SetInt("Food", InitialFood);
        PlayerPrefs.SetInt("Villagers", InitialVillages);
        PlayerPrefs.SetInt("Scraps", InitialScrap);
        PlayerPrefs.SetInt("Premium", InitialPremium + (CampManager.Instance.Upgrades.MaxVillages*2));
    }
}
