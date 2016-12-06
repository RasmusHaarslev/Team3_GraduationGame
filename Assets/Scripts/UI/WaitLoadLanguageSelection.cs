using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitLoadLanguageSelection : MonoBehaviour {

    public GameObject languagePanel;

    [Range(0,5)]
    public float fadingDuration = 2;

    void Start()
    {
        PlayerPrefs.SetInt(StringResources.LevelDifficultyPrefsName, 4);
        Invoke("StartCutscene", 3.0f);
    }
   
    private void StartCutscene()
    {
        StartCoroutine(FadeOut(fadingDuration));
    }

    public IEnumerator FadeOut(float duration)
    {
        Image sprite = GetComponent<Image>();

        while (sprite.color.a > 0)
        {
            Color newColor = sprite.color;
            newColor.a -= Time.deltaTime / duration;
            sprite.color = newColor;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
