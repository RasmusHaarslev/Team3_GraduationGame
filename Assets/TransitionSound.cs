using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class TransitionSound : MonoBehaviour {

    private int currentScene;

	void Start () {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        DontDestroyOnLoad(this.gameObject);

        Invoke("StartCutscene", 3.0f);
    }

    private void StartCutscene()
    {
        AkSoundEngine.PostEvent("Play_GroundAmbience", Camera.main.gameObject);
    }

    void Awake()
    {
        SceneManager.activeSceneChanged += MyMethod; // subscribe
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= MyMethod; // unsubscribe
    }

    private void MyMethod(Scene arg0, Scene arg1)
    {
        Destroy(this.gameObject);
    }
}
