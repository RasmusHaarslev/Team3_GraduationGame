using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartSceneTransition : MonoBehaviour {
    public float transitionTime = 1.0f;

    Image fadeImg;

    float time = 1.0f;

    void Start() {
        fadeImg = gameObject.GetComponent<Image>();
        //StartCoroutine(StartScene());
    }


	void Update()
	{
		if(time <= 0.0f)
		{
			gameObject.SetActive(false);
		}

		fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, time);
		time -= Time.deltaTime * (1.0f / transitionTime);
	}
    IEnumerator StartScene()
    {
        while (time >= 0.0f)
        {
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, time);
            time -= Time.deltaTime * (1.0f / transitionTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}