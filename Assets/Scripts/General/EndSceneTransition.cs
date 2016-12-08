using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndSceneTransition : MonoBehaviour
{
    public float transitionTime = 1.0f;

    Image fadeImg;

    float time = 0f;

    public Boolean loadLevel = false;

    void OnEnable()
    {
        EventManager.Instance.StartListening<EndSceneTransitionEvent>(StartTransition);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<EndSceneTransitionEvent>(StartTransition);
    }

    void OnApplicationQuit()
    {
        this.enabled = false;
    }

    void Start()
    {
        fadeImg = gameObject.GetComponent<Image>();
    }

    public void StartTransition(EndSceneTransitionEvent e)
    {
        fadeImg.enabled = true;
        StartCoroutine(EndScene(e.scene));
    }

    public void StartTransition(string scene) {
        fadeImg.enabled = true;
        StartCoroutine(EndScene(scene));
    }

    IEnumerator EndScene(string scene)
    {
        fadeImg.gameObject.SetActive(true);
        time = 0.0f;
        yield return null;
        while (time <= 1.0f)
        {
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, time);
            time += Time.deltaTime * (1.0f / transitionTime);
            yield return null;
        }
        if (loadLevel)
            GameController.Instance.LoadLevel();
        else
            GameController.Instance.LoadScene(scene);
    }
}