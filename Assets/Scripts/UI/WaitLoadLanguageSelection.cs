using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitLoadLanguageSelection : MonoBehaviour {

    [Range(0,5)]
    public float fadingDuration = 2;

    public float fadeDelay = 3.0f;

    public GameObject nextPanel;

    void Start()
    {
        PlayerPrefs.SetInt(StringResources.LevelDifficultyPrefsName, 4);
        Invoke("StartCutscene", fadeDelay);
    }
   
    private void StartCutscene()
    {
        nextPanel.SetActive(true);
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

    public void Skip()
    {
        CancelInvoke();
        nextPanel.SetActive(true);
        StartCoroutine(FadeOut(fadingDuration));
    }
}
