using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    private float currentMusicVolume = 0.8f;
    private float currentFXVolume = 0.8f;

    public Text MenuHeader;
    public GameObject Music;
    public GameObject FX;

    public Text txtResourcesFoundHeader;
    public Text txtResourcesFound;

    void Start()
    {
        AdjustMusicVolume();
        AdjustFXVolume();

        Manager_Audio.SendParameterValue(Manager_Audio.adjustMusicVolume, currentMusicVolume);
        Manager_Audio.SendParameterValue(Manager_Audio.adjustFXVolume, currentFXVolume);
        UpdateText();
    }

	void OnEnable () {
        UpdateText();        

        if (PlayerPrefs.GetInt("MusicToggle") == 0) { 
            Music.transform.GetChild(0).GetComponent<Toggle>().isOn = false;
        } else {
            Music.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        }

        if (PlayerPrefs.GetInt("FXToggle") == 0)
        {
            FX.transform.GetChild(0).GetComponent<Toggle>().isOn = false;
        }
        else
        {
            FX.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        }


        Music.transform.GetChild(1).GetComponent<Slider>().value = Manager_Audio.musicValue;
        FX.transform.GetChild(1).GetComponent<Slider>().value = Manager_Audio.fxValue;
    }

    public void UpdateText()
    {
        MenuHeader.text = TranslationManager.Instance.GetTranslation("Menu");
        Music.transform.GetChild(2).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Music");
        FX.transform.GetChild(2).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("FX");

        if(txtResourcesFound != null)
        {
            txtResourcesFoundHeader.text = TranslationManager.Instance.GetTranslation("LevelCleared");
            txtResourcesFound.text = TranslationManager.Instance.GetTranslation("ResourcesFound");
        }
    }

    #region MUSIC OPTIONS
    public void ToggleMusic()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        Manager_Audio.musicToggle = !Manager_Audio.musicToggle;

        if (Manager_Audio.musicToggle)
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustMusicVolume, currentMusicVolume);
            PlayerPrefs.SetInt("MusicToggle", 1);
        }
        else
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustMusicVolume, 0);
            PlayerPrefs.SetInt("MusicToggle", 0);
        }
    }

    public void AdjustMusicVolume()
    {
        currentMusicVolume = Music.transform.GetChild(1).GetComponent<Slider>().value;
        Manager_Audio.SendParameterValue(Manager_Audio.adjustMusicVolume, currentMusicVolume);
        Manager_Audio.musicValue = currentMusicVolume;
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
            PlayerPrefs.SetInt("FXToggle", 1);
        }
        else
        {
            Manager_Audio.SendParameterValue(Manager_Audio.adjustFXVolume, 0);
            PlayerPrefs.SetInt("FXToggle", 0);
        }        
    }

    public void AdjustFXVolume()
    {
        currentFXVolume = FX.transform.GetChild(1).GetComponent<Slider>().value;
        Manager_Audio.SendParameterValue(Manager_Audio.adjustFXVolume, currentFXVolume);
        Manager_Audio.fxValue = currentFXVolume;
    }
    #endregion
}
