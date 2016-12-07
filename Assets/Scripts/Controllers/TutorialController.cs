using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TutorialController: MonoBehaviour
{
    public TutorialType Type;

    private bool GameplayTutorialCompleted;
    private bool CampTutorialCompleted;
    private bool LevelSelectionTutorialCompleted;
    private bool SoldierTutorialCompleted;
    private bool UpgradesTutorialCompleted;
    private bool RecruitTutorialCompleted;

    void Awake()
    {
        if (!GameplayTutorialCompleted)
        {
            GameplayTutorialCompleted = PlayerPrefs.GetInt("GameplayTutorialCompleted") == 1 ? true : false;
            CampTutorialCompleted = PlayerPrefs.GetInt("CampTutorialCompleted") == 1 ? true : false;
            LevelSelectionTutorialCompleted = PlayerPrefs.GetInt("LevelSelectionTutorialCompleted") == 1 ? true : false;
            SoldierTutorialCompleted = PlayerPrefs.GetInt("SoldierTutorialCompleted") == 1 ? true : false;
            UpgradesTutorialCompleted = PlayerPrefs.GetInt("UpgradesTutorialCompleted") == 1 ? true : false;
            RecruitTutorialCompleted = PlayerPrefs.GetInt("RecruitTutorialCompleted") == 1 ? true : false;
        }
    }

    void Start()
    {
        switch (Type)
        {
            case TutorialType.GameplayTutorial:
                if (GameplayTutorialCompleted)
                    this.gameObject.SetActive(false);
                break;

            case TutorialType.CampTutorial:
                if (CampTutorialCompleted)
                    this.gameObject.SetActive(false);
                break;

            case TutorialType.LevelSelectionTutorial:
                if (LevelSelectionTutorialCompleted)
                    this.gameObject.SetActive(false);
                break;

            case TutorialType.SoldierTutorial:
                if (SoldierTutorialCompleted)
                    this.gameObject.SetActive(false);
                break;

            case TutorialType.UpgradesTutorial:
                if (UpgradesTutorialCompleted)
                    this.gameObject.SetActive(false);
                break;

            case TutorialType.RecruitTutorial:
                if (RecruitTutorialCompleted)
                    this.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    public void TutorialSeen()
    {
        switch (Type)
        {
            case TutorialType.GameplayTutorial:
                PlayerPrefs.SetInt("GameplayTutorialCompleted", 1);
                break;

            case TutorialType.CampTutorial:
                PlayerPrefs.SetInt("CampTutorialCompleted",1);
                break;

            case TutorialType.LevelSelectionTutorial:
                PlayerPrefs.SetInt("LevelSelectionTutorialCompleted",1);
                break;

            case TutorialType.SoldierTutorial:
                PlayerPrefs.SetInt("SoldierTutorialCompleted",1);
                break;

            case TutorialType.UpgradesTutorial:
                PlayerPrefs.SetInt("UpgradesTutorialCompleted",1);
                break;

            case TutorialType.RecruitTutorial:
                PlayerPrefs.SetInt("RecruitTutorialCompleted",1);
                break;

            default:
                Debug.Log("Something went wrong!");
                break;
        }
    }
}

public enum TutorialType
{
    CampTutorial,
    GameplayTutorial,
    SoldierTutorial,
    UpgradesTutorial,
    RecruitTutorial,
    LevelSelectionTutorial
}