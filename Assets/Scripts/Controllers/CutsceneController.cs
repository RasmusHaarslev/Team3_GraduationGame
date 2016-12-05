using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public string cutscene;
    public string scene;
    public Boolean randomLevel;

    void Start()
    {
        Invoke("StartCutscene", 3.0f);
    }

	// Use this for initialization
	private void StartCutscene ()
	{
        Handheld.PlayFullScreenMovie(cutscene, Color.black, FullScreenMovieControlMode.CancelOnInput);

	    if (randomLevel)
            GameController.Instance.LoadLevel();
        else
            GameController.Instance.LoadScene(scene);
    }
}
