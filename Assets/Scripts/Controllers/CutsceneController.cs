using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    void Start()
    {
        Invoke("StartCutscene", 3.0f);
    }

	// Use this for initialization
	private void StartCutscene ()
	{
        Handheld.PlayFullScreenMovie("soundtest.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        GameController.Instance.LoadScene("TutorialLevel01");
    }
}
