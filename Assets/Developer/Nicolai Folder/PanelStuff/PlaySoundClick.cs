using UnityEngine;
using System.Collections;

public class PlaySoundClick : MonoBehaviour {

	public void PlaySound()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
    }
}
