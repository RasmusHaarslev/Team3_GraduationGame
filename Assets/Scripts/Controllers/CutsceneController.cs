using UnityEngine;

public class CutsceneController : MonoBehaviour
{

	public GameObject blackScreen;

    void Start()
    {
        Invoke("StartCutscene", 3.0f);
    }

	// Use this for initialization
	private void StartCutscene ()
	{
		blackScreen.SetActive(true);
        Handheld.PlayFullScreenMovie("soundtest.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        GameController.Instance.LoadScene("TutorialLevel01");
    }
}
