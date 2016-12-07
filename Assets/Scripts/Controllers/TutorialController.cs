using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TutorialController: MonoBehaviour
{
    public TutorialType Type;
    private TutorialDatabase database;

    void Awake()
    {
        if (!database)
        {
            database = Resources.Load("ScriptableObjects/TutorialDatabase") as TutorialDatabase;
        }
    }

    void Start()
    {
        switch (Type)
        {
            case TutorialType.GameplayTutorial:
                if (database.GameplayTutorialCompleted)
                {
                    this.gameObject.SetActive(false);
                }
                break;

            case TutorialType.CampTutorial:
                if (database.CampTutorialCompleted)
                {
                    this.gameObject.SetActive(false);
                }
                break;

            case TutorialType.LevelSelectionTutorial:
                if (database.LevelSelectionTutorialCompleted)
                {
                    this.gameObject.SetActive(false);
                }
                break;

            case TutorialType.SoldierTutorial:
                if (database.SoldierTutorialCompleted)
                {
                    this.gameObject.SetActive(false);
                }
                break;

            case TutorialType.UpgradesTutorial:
                if (database.UpgradesTutorialCompleted)
                {
                    this.gameObject.SetActive(false);
                }
                break;

            case TutorialType.RecruitTutorial:
                if (database.RecruitTutorialCompleted)
                {
                    this.gameObject.SetActive(false);
                }
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
                database.GameplayTutorialCompleted = true;
                break;

            case TutorialType.CampTutorial:
                database.CampTutorialCompleted = true;
                break;

            case TutorialType.LevelSelectionTutorial:
                database.LevelSelectionTutorialCompleted = true;
                break;

            case TutorialType.SoldierTutorial:
                database.SoldierTutorialCompleted = true;
                break;

            case TutorialType.UpgradesTutorial:
                database.UpgradesTutorialCompleted = true;
                break;

            case TutorialType.RecruitTutorial:
                database.RecruitTutorialCompleted = true;
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