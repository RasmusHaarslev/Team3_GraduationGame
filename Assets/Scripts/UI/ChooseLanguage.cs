using UnityEngine;
using System.Collections;

public class ChooseLanguage : MonoBehaviour {

    public string danishCutScene;
    public string englishCutScene;

    void Awake()
    {
        
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
        if (!(PlayerPrefs.GetInt("GameplayTutorialCompleted") == 1)) { 
            //Handheld.PlayFullScreenMovie(cutscene + ".mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
            
            PlayerPrefs.SetInt(StringResources.LevelDifficultyPrefsName, 4);
            
            //ClearAllScriptableObject();
            EventManager.Instance.TriggerEvent(new EndSceneTransitionEvent("TrueIntroCutscene"));
            //GameController.Instance.LoadScene("TrueIntroCutscene");
        }
        else
        {
            EventManager.Instance.TriggerEvent(new EndSceneTransitionEvent("CampManagement"));
            //GameController.Instance.LoadScene("CampManagement");
        }
    }

    public void ClearAllScriptableObject()
    {
        DataService dataService = new DataService(StringResources.databaseName);
        dataService.ResetDatabase();
    }
}
