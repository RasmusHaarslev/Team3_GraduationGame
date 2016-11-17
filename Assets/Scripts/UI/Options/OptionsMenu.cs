using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public Text MenuHeader;
    public GameObject OverAllSound;
    public GameObject Music;
    public GameObject FX;
    public GameObject LanguageHeader;

    public GameObject ToggleDanish;
    
	void OnEnable () {
        UpdateText();
    }

    public void UpdateText()
    {
        MenuHeader.text = TranslationManager.Instance.GetTranslation("Menu");
        OverAllSound.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("GeneralSound");
        Music.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Music");
        FX.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("FX");
        LanguageHeader.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("LanguageHeader");
    }

    public void ToggleSound()
    {
        Manager_Audio.overAllSoundToggle = !Manager_Audio.overAllSoundToggle;

        if (Manager_Audio.overAllSoundToggle)
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustOverallVolume, Manager_Audio.currentOverAllSoundVolume);
        }
        else
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustOverallVolume, 0);
        }
    }

    public void AdjustSound()
    {
        Manager_Audio.currentOverAllSoundVolume = OverAllSound.transform.GetChild(1).GetComponent<Slider>().value;
    }

    public void ChangeLanguage()
    {
        if (ToggleDanish.GetComponent<Toggle>().isOn)
        {
            TranslationManager.Instance.LoadLanguage(false);
        } else
        {
            TranslationManager.Instance.LoadLanguage(true);
        }

        UpdateText();
    }
}
