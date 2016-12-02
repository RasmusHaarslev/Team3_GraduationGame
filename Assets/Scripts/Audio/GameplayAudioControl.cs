using UnityEngine;
using System.Collections;

public class GameplayAudioControl : MonoBehaviour {

	void Start()
	{
		EnableExploreStuffAudio ();
	}

	void OnDisable()
	{
		DisableExploreStuffAudio ();
	}

	public void EnableExploreStuffAudio()
	{
		Manager_Audio.PlaySound(Manager_Audio.musicExploreStart, this.gameObject);
		Manager_Audio.PlaySound(Manager_Audio.baseAmbiencePlay, this.gameObject);
	}

	public void DisableExploreStuffAudio()
	{
		Manager_Audio.PlaySound(Manager_Audio.baseAmbienceStop, this.gameObject);
		Manager_Audio.PlaySound(Manager_Audio.musicExploreStop, this.gameObject);
	}
}
