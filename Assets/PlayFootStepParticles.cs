using UnityEngine;
using System.Collections;

public class PlayFootStepParticles : MonoBehaviour {
	[SerializeField] ParticleSystem leftFoot,rightFoot;
	// Use this for initialization
	public bool hasFootStepParticles = true;

	void Start () 
	{
	
	}
	
	public void PlayLeftStep()
	{
		if(hasFootStepParticles)
		{
			leftFoot.Play ();
		}
	}

	public void PlayRightStep()
	{
		if(hasFootStepParticles)
		{
			rightFoot.Play ();
		}
	}
}
