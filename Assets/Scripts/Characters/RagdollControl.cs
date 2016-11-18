using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollControl : MonoBehaviour
{
	private Animator anim;

	[Tooltip("Colliders used for the ragdoll. Script collects them from children...")]
	private List<Collider> collidersRagdoll;

	private List<Rigidbody> rigidBodiesRagdoll;


	[Tooltip("Collect player collider on main object")]
	private Collider playerCollider;
	[SerializeField]
	private CapsuleCollider playerCapsuleCollider;
	[SerializeField]
	private SphereCollider playerSphereCollider;

	#region System
	void Awake()
	{
		anim = GetComponent<Animator>();
		playerCollider = GetComponent<Collider>();
		CollectColliders();
		CollectRigidBodies();
	}

	void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			EnableRagDoll();
		}
	}

	#endregion System

	#region Custom

	public void EnableRagDoll()
	{
		if (playerCollider != null)
		{
			playerCollider.isTrigger = true;
		}

		ToggleColliders(true);
		ToggleRigids(false);
		anim.enabled = false;
	}

	public void DisableRagDoll()
	{
		if (playerCollider != null)
		{
			playerCollider.isTrigger = false;
		}

		// enable colliders
		ToggleColliders(false);

		anim.enabled = true;
	}

	void CollectColliders()
	{
		collidersRagdoll = new List<Collider>();

		if (playerCollider != null)
		{
			foreach (var collider in GetComponentsInChildren<Collider>())
			{
				if (collider != playerCapsuleCollider && collider != playerSphereCollider)
				{
					collidersRagdoll.Add(collider);
				}
			}
		}
		else
		{
			collidersRagdoll.AddRange(GetComponentsInChildren<Collider>());
		}

		ToggleColliders(false);
	}

	void CollectRigidBodies()
	{
		rigidBodiesRagdoll = new List<Rigidbody>();

		rigidBodiesRagdoll.AddRange(GetComponentsInChildren<Rigidbody>());

		ToggleRigids(true);
	}

	void ToggleColliders(bool isEnabled)
	{
		for (int i = 0; i < collidersRagdoll.Count; i++)
		{
			collidersRagdoll[i].enabled = isEnabled;
		}
	}

	void ToggleRigids(bool isEnabled)
	{
		for (int i = 0; i < rigidBodiesRagdoll.Count; i++)
		{
			rigidBodiesRagdoll[i].isKinematic = isEnabled;
		}
	}

	#endregion Custom

}
