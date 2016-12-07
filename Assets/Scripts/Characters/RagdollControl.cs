using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollControl : MonoBehaviour
{
	private Animator anim;

	[Tooltip("Colliders used for the ragdoll. Script collects them from children...")]
	private List<Collider> collidersRagdoll;

	private List<Rigidbody> rigidBodiesRagdoll;
	public Rigidbody rigidbody;

	[Tooltip("Collect player collider on main object")]
	private Collider playerCollider;
	[SerializeField]
	private CapsuleCollider playerCapsuleCollider;
	[SerializeField]
	private SphereCollider playerSphereCollider;
	[SerializeField]
	private Collider tapCollider;
	[SerializeField]
	private float pushForceOnDeath=10;
	public ForceMode forceModePush = ForceMode.Impulse;
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

		foreach (var rigid in rigidBodiesRagdoll)
		{
				rigid.WakeUp();
		}

		ToggleColliders(true);
		ToggleRigids(false);
		anim.enabled = false;
		rigidbody.AddForce(-transform.forward*pushForceOnDeath,forceModePush);
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
				if (collider != playerCapsuleCollider && collider != playerSphereCollider && collider != tapCollider)
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
		foreach (var rigid in GetComponentsInChildren<Rigidbody>())
		{
			if (rigid != rigidbody)
			{
				rigidBodiesRagdoll.Add(rigid);
				rigid.Sleep();
			}
		}
		
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
