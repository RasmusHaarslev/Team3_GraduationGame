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
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		{
			Shoot ();
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
