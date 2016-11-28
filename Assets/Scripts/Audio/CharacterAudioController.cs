using UnityEngine;
using System.Collections;

public class CharacterAudioController : MonoBehaviour {

	public bool hasFootsteps = true;
	public bool hasWeaponSounds = true;

	public void PlayFootStep () 
	{
		if(hasFootsteps)
		{
			Manager_Audio.PlaySound (Manager_Audio.leaderFootStep,this.gameObject);
		}
	}

	public void PlaySpear () 
	{
		if(hasWeaponSounds)
		{
			Manager_Audio.PlaySound (Manager_Audio.attackSpear,this.gameObject);
		}
	}

	public void PlayRifle () 
	{
		if(hasWeaponSounds)
		{
			Manager_Audio.PlaySound (Manager_Audio.attackRiffle,this.gameObject);
		}
	}

	public void PlayShield () 
	{
		if(hasWeaponSounds)
		{
			Manager_Audio.PlaySound (Manager_Audio.attackShield,this.gameObject);
		}
	}
}
