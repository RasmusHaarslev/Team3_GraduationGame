using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TribesLeft : MonoBehaviour {

    int campsLeft;

    void OnEnable()
    {
        EventManager.Instance.StartListening<ClearedCampEvent>(UpdateText);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<ClearedCampEvent>(UpdateText);
    }

	void Start () {
        campsLeft = PlayerPrefs.GetInt(StringResources.TribeCampsPrefsName, 4);
        GetComponent<Text>().text = campsLeft.ToString() + " " + TranslationManager.Instance.GetTranslation("TribesLeft");
	}

    public void UpdateText(ClearedCampEvent e)
    {
        campsLeft -= 1;
        GetComponent<Text>().text = campsLeft.ToString() + " " + TranslationManager.Instance.GetTranslation("TribesLeft");
    }
}
