using UnityEngine;
using System.Collections;

public class MenuMusicControl : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
        EnableCampAudio ();
	}
	
	// Update is called once per frame
	void OnDisable () 
	{
		DisableCampAudio ();
	}

	public void EnableCampAudio()
	{
		//Manager_Audio.PlaySound(Manager_Audio.play_menuMusic, gameObject);
		Manager_Audio.PlaySound(Manager_Audio.play_menuAmbience, gameObject);
	}

	public void DisableCampAudio()
	{
		//Manager_Audio.PlaySound(Manager_Audio.stop_menuMusic, gameObject);
		Manager_Audio.PlaySound(Manager_Audio.stop_menuAmbience, gameObject);
	}
}
