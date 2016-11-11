using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShootControl : MonoBehaviour {
	#region Variables
	[SerializeField] private Transform shootPoint;
	[SerializeField] private Rigidbody projectilePrefab;
	[SerializeField] private float projectileSpeed;
	[SerializeField] private int amountBullets;
	[SerializeField] private List<Rigidbody> projectilesPool = new List<Rigidbody>();
	[SerializeField] private float shootRate;
	[SerializeField] private string shootButton = "Jump";
	float nextFireTime;
	#endregion

	#region System Methods
	// Use this for initialization
	void Awake () {
		CreateBulletPool ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton(shootButton))
		{
			Shoot ();
		}
	}

	#endregion


	#region Custom Methods

	public void Shoot()
	{
		if(nextFireTime<Time.time)
		{
			print ("shoot");

			nextFireTime = Time.time + shootRate;
			GetBullet ().AddForce (shootPoint.transform.forward*projectileSpeed);
		}

	}

	public Rigidbody GetBullet()
	{
		Rigidbody projectileInstance = new Rigidbody();
		for(int i = 0;i<amountBullets;i++)
		{
			if(projectilesPool[i].gameObject.activeInHierarchy == false)
			{
				
				projectilesPool [i].gameObject.transform.position = shootPoint.position;
				projectilesPool [i].gameObject.SetActive (true);
				projectileInstance = projectilesPool [i];
				break;
			}
		}
		return projectileInstance;
	}

	void CreateBulletPool()
	{
		for(int i = 0;i<amountBullets;i++)
		{
			Rigidbody rigid = Instantiate (projectilePrefab) as Rigidbody;
			projectilesPool.Add (rigid);
			//rigid.transform.parent = this.transform;
			rigid.gameObject.SetActive (false);
		}
	}


	#endregion
}
