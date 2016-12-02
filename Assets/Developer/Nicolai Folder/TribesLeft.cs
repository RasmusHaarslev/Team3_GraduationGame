using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TribesLeft : MonoBehaviour {

    int campsLeft;

    void OnEnable()
    {
        EventManager.Instance.StartListening<ClearedCampEvent>(UpdateCampsLeft);
        EventManager.Instance.StartListening<LanguageChanged>(UpdateText);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<ClearedCampEvent>(UpdateCampsLeft);
        EventManager.Instance.StopListening<LanguageChanged>(UpdateText);
    }

    void OnApplicationQuit()
    {
        this.enabled = false;
    }

    void Start () {
        campsLeft = PlayerPrefs.GetInt(StringResources.TribeCampsPrefsName, 4);
        GetComponent<Text>().text = campsLeft.ToString() + " " + TranslationManager.Instance.GetTranslation("TribesLeft");
	}

    public void UpdateCampsLeft(ClearedCampEvent e)
    {
        campsLeft -= 1;
        GetComponent<Text>().text = campsLeft.ToString() + " " + TranslationManager.Instance.GetTranslation("TribesLeft");
    }

    public void UpdateText(LanguageChanged e)
    {    
        GetComponent<Text>().text = campsLeft.ToString() + " " + TranslationManager.Instance.GetTranslation("TribesLeft");
    }
}
