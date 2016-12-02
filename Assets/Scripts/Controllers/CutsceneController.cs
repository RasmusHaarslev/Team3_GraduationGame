using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    void Start()
    {
        Invoke("StartCutscene", 3.0f);
    }

	// Use this for initialization
	private void StartCutscene ()
	{
        Handheld.PlayFullScreenMovie("Cutscene1.mp4", Color.black, FullScreenMovieControlMode.Full);
        GameController.Instance.LoadScene("TutorialLevel01");
    }
}
