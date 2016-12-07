using UnityEngine;
using System.Collections;

public class ChooseLanguage : MonoBehaviour {

    public string danishCutScene;
    public string englishCutScene;

    private TutorialDatabase tutorialDb;
    private UpgradesDatabase upgradesDb;

    void Awake()
    {
        tutorialDb = Resources.Load("ScriptableObjects/TutorialDatabase") as TutorialDatabase;
        upgradesDb = Resources.Load("ScriptableObjects/UpgradesDatabase") as UpgradesDatabase;
    }

    public void SetLanguage(int languageID)
    {        
        if (languageID == 0) { 
            PlayMedia(danishCutScene);
            TranslationManager.Instance.LoadLanguage(false);
        } else {
            PlayMedia(englishCutScene);
            TranslationManager.Instance.LoadLanguage(true);
        }
    }

    public void PlayMedia(string cutscene)
    {
        if (!tutorialDb.GameplayTutorialCompleted) { 
            Handheld.PlayFullScreenMovie(cutscene + ".mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
            tutorialDb.GameplayTutorialCompleted = true;
            PlayerPrefs.SetInt(StringResources.LevelDifficultyPrefsName, 4);
            ClearAllScriptableObject();
            GameController.Instance.LoadScene("TutorialLevel01");
        }
        else
        {
            GameController.Instance.LoadScene("CampManagement");
        }
    }

    public void ClearAllScriptableObject()
    {
        DataService dataService = new DataService(StringResources.databaseName);
        dataService.ResetDatabase();

        upgradesDb.BlacksmithLevel = 1;
        upgradesDb.GatherLevel = 1;
        upgradesDb.LeaderHealthLevel = 1;
        upgradesDb.LeaderStrengthLevel = 1;

        
    }
}
