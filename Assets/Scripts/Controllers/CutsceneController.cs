using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour {

    void Start()
    {
        Invoke("StartCutscene", 1.0f);
    }

	// Use this for initialization
	public void StartCutscene () {
        Debug.Log("hej");
        Handheld.PlayFullScreenMovie("NewTest.mp4", Color.black, FullScreenMovieControlMode.Full);
    }
}
