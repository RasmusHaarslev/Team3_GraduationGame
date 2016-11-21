using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    private float currentMusicVolume = 0.8f;
    private float currentFXVolume = 0.8f;

    public Text MenuHeader;
    public GameObject Music;
    public GameObject FX;
    public GameObject LanguageHeader;

    public GameObject ToggleDanish;
    
    void Start()
    {
        AdjustMusicVolume();
        Manager_Audio.SendParameterValue(Manager_Audio.adjustMusicVolume, currentMusicVolume);
        Manager_Audio.SendParameterValue(Manager_Audio.adjustFXVolume, currentFXVolume);
    }

	void OnEnable () {
        UpdateText();
    }

    public void UpdateText()
    {
        MenuHeader.text = TranslationManager.Instance.GetTranslation("Menu");
        Music.transform.GetChild(2).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Music");
        FX.transform.GetChild(2).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("FX");
        LanguageHeader.transform.GetChild(0).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("LanguageHeader");
        LanguageHeader.transform.GetChild(1).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("English");
        LanguageHeader.transform.GetChild(2).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Danish");
    }
    #region MUSIC OPTIONS
    public void ToggleMusic()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        Manager_Audio.musicToggle = !Manager_Audio.musicToggle;

        if (Manager_Audio.musicToggle)
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustMusicVolume, currentMusicVolume);
        }
        else
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustMusicVolume, 0);
        }
    }

    public void AdjustMusicVolume()
    {
        currentMusicVolume = Music.transform.GetChild(1).GetComponent<Slider>().value;
        Manager_Audio.SendParameterValue(Manager_Audio.adjustMusicVolume, currentMusicVolume);
    }
    #endregion

    #region FX OPTIONS
    public void ToggleFX()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        Manager_Audio.fxToggle = !Manager_Audio.fxToggle;

        if (Manager_Audio.fxToggle)
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustFXVolume, currentFXVolume);
        }
        else
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustFXVolume, 0);
        }
    }

    public void AdjustFXVolume()
    {
        currentFXVolume = FX.transform.GetChild(1).GetComponent<Slider>().value;
        Manager_Audio.SendParameterValue(Manager_Audio.adjustFXVolume, currentFXVolume);
    }
    #endregion

    public void ChangeLanguage()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
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
