using UnityEngine;
using System.Collections;

public class CharacterAudioController : MonoBehaviour {

	public void PlayFootStep () 
	{
		Manager_Audio.PlaySound (Manager_Audio.leaderFootStep,this.gameObject);
	}
}
