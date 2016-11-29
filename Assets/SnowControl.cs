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
	public float littleAmount=3,midAmount = 10,heavyAmount = 20;

	// Use this for initialization
	void OnEnable () {
		switch(_snowAmount)
		{
		case SnowAmount.little:
			ChangeSnowAmountParticles (littleAmount);
			break;
		case SnowAmount.mid:
			ChangeSnowAmountParticles (midAmount);
			break;
		case SnowAmount.heavy:
			ChangeSnowAmountParticles (heavyAmount);
			break;
		}
	}
	
	// Update is called once per frame
	void ChangeSnowAmountParticles (float amount) {

//		ParticleSystem.EmissionModule emission;
//
//		emission.rate = amount;

		for(int i = 0;i<particles.Length;i++)
		{
			var em = particles [i].emission;

			em.rate = amount;
		}
	}
}
