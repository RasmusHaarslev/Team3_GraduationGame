using UnityEngine;
using System.Collections;

public class ChooseLanguage : MonoBehaviour {

    public string danishCutScene;
    public string englishCutScene;

    public void ChoseLanguage(int languageID)
    {        
        PlayerPrefs.SetInt(StringResources.Language, languageID);

        if (languageID == 0) { 
            PlayMedia(danishCutScene);
        } else {
            PlayMedia(englishCutScene);
        }
    }

    public void PlayMedia(string cutscene)
    {
        Handheld.PlayFullScreenMovie(cutscene + ".mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);

        GameController.Instance.LoadScene("TutorialLevel01");
    }
}
