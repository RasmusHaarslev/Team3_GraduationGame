using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollControl : MonoBehaviour {
	private Animator anim;

	[Tooltip("Colliders used for the ragdoll. Script collects them from children...")]
	private List<Collider> collidersRagdoll;

	[Tooltip("Collect player collider on main object")]
	private Collider playerCollider;

	#region System
	void Awake () {
		anim = GetComponent<Animator> ();
		playerCollider = GetComponent<Collider> ();
		CollectColliders ();
	}

	void Update()
	{
		if(Input.GetButtonDown("Jump"))
		{
			EnableRagDoll ();
		}
	}

	#endregion System

	#region Custom

	public void EnableRagDoll()
	{
		if(playerCollider != null)
		{
			playerCollider.isTrigger = true;
		}

		// enable colliders
		ToggleColliders (true);

		anim.enabled = false;
	}

	public void DisableRagDoll()
	{
		if(playerCollider != null)
		{
			playerCollider.isTrigger = false;
		}

		// enable colliders
		ToggleColliders (false);

		anim.enabled = true;
	}

	void CollectColliders()
	{
		collidersRagdoll = new List<Collider> ();

		if (playerCollider != null) {
			foreach (var collider in GetComponentsInChildren<Collider> ()) {
				if (collider != playerCollider) {
					collidersRagdoll.Add (collider);
				}
			}
		} else {
			collidersRagdoll.AddRange (GetComponentsInChildren<Collider> ());
		}

		ToggleColliders (false);
	}

	void ToggleColliders(bool isEnabled)
	{
		for(int i = 0;i<collidersRagdoll.Count;i++)
		{
			collidersRagdoll [i].enabled = isEnabled;
		}
	}

	#endregion Custom

}
