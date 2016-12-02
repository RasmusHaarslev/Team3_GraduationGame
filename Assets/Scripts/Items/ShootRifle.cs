using UnityEngine;
using System.Collections;

public class ShootRifle : MonoBehaviour {

	[SerializeField]ParticleSystem[] muzzles;
	// Use this for initialization
	void OnEnable () 
	{
		muzzles = GetComponentsInChildren<ParticleSystem> ();

		for(int i =0;i<muzzles.Length;i++)
		{
			muzzles[i].Stop ();
		}
	}
	
	public void Shoot()
	{
		for(int i =0;i<muzzles.Length;i++)
		{
			muzzles[i].Play ();
		}
	}
}
