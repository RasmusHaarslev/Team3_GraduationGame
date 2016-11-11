using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	public float damageAmount=2;
	public float bulletLife=2;
	public Rigidbody rigid;

	void OnEnable () 
	{
		Invoke ("DisableBullet",bulletLife);
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider c) {

		CancelInvoke ();

		CheckDamage (c);

		DisableBullet ();
	}

	void CheckDamage(Collider c)
	{
		IDamage damageInterface = c.GetComponent<IDamage> ();
		if(damageInterface != null)
		{
			damageInterface.Damage (damageAmount);
		}
	}

	void DisableBullet()
	{
		rigid.velocity = Vector3.zero;
		this.gameObject.SetActive (false);
	}
}
