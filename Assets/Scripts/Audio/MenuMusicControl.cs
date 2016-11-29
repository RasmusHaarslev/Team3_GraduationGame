using UnityEngine;
using System.Collections;

public class MenuMusicControl : MonoBehaviour {

	// Use this for initialization
	void OnEnable () 
	{
		Manager_Audio.PlaySound(Manager_Audio.play_menuMusic, gameObject);
		Manager_Audio.PlaySound(Manager_Audio.play_menuAmbience, gameObject);
	}
	
	// Update is called once per frame
	void OnDisable () 
	{
		Manager_Audio.PlaySound(Manager_Audio.stop_menuMusic, gameObject);
		Manager_Audio.PlaySound(Manager_Audio.stop_menuAmbience, gameObject);
	}
}
