using UnityEngine;
using System.Collections;



public class SnowControl : MonoBehaviour {

	public enum SnowAmount
	{
		little,
		mid,
		heavy
	}

	public SnowAmount _snowAmount;
	public ParticleSystem[] particles;
	[SerializeField] float maxSnowAmount=100;

	// Use this for initialization
	void OnEnable () 
	{
		HandleParticlesOnEnable (_snowAmount);
	}

	public void HandleParticlesOnEnable(SnowAmount _snowAmount)
	{
		switch(_snowAmount)
		{
		case SnowAmount.little:
			ChangeSnowAmountParticles (maxSnowAmount*0.3f);
			break;
		case SnowAmount.mid:
			ChangeSnowAmountParticles (maxSnowAmount*0.7f);
			break;
		case SnowAmount.heavy:
			ChangeSnowAmountParticles (maxSnowAmount);
			break;
		}
	}

	public void ChangeSnowAmountParticles (float amount) 
	{
		for(int i = 0;i<particles.Length;i++)
		{
			var em = particles [i].emission;
			em.rate = amount;
		}
	}
}
