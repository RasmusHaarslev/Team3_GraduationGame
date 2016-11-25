﻿using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
	public GameObject player;

    #region Inspector fields
    [Tooltip("Defines the Field of view (could just be on the cam?)")]
    public float FieldOfView = 53.58f;
    [Tooltip("Sets the distance away from the player")]
	public float Distance = 15f;
	[Tooltip("Sets the height relative to the player")]
	public float Height = 8f;
	[Tooltip("Sets amount of slerp (higher = faster)")]
	public float MoveSlerpAmount = 1f;
	[Tooltip("Sets amount of slerp (higher = faster)")]
	public float RotationSlerpAmount = 1f;
	[HideInInspector,Tooltip("Sets offset for the camera look direction, relative to the players direction")]
	public float CameraOffset = 1f;
	#endregion

	#region Hidden public fields
	[HideInInspector]
	public Boolean OverridePosition = false;
	[HideInInspector]
	public Vector3 OverriddenPosition = new Vector3();

	[HideInInspector]
	public Boolean OverrideDistance = false;
	[HideInInspector]
	public float OverriddenDistance = 0f;

	[HideInInspector]
	public Boolean OverrideHeight = false;
	[HideInInspector]
	public float OverriddenHeight = 0f;

	[HideInInspector]
	public Boolean OverrideSlerp = false;
	[HideInInspector]
	public float OverriddenSlerp = 0f;
    [HideInInspector]
    public Boolean OverrideRotationSlerp = false;
    [HideInInspector]
    public float OverriddenRotationSlerp = 0f;

    [HideInInspector]
	public float XRotationOffset = 0f;
    [HideInInspector]
	public bool SlerpBack;
	#endregion

	#region Hidden private fields
	float height;
	float distance;
	float slerp;
    float rotationSlerp;
    private Camera mainCamera;
    #endregion

    // Use this for initialization
    void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update()
	{
		if (player != null)
		{
			TransformPosition();
			TransformLook();
		}
		else
		{
			player = GameObject.FindGameObjectWithTag("Player");
            mainCamera = this.GetComponent<Camera>();
        }
	}

	void TransformPosition()
	{
        if (Math.Abs(distance - Distance) < 0.01f && Math.Abs(height - Height) < 0.01f)
            SlerpBack = false;

		SetSlerp();
		SetHeight();
		SetDistance();

		Vector3 clickedTarget = player.GetComponent<NavMeshAgent>().destination;
		Vector3 offset = player.transform.forward.normalized * CameraOffset;

		Vector3 positionTarget = OverridePosition ? OverriddenPosition : player.transform.position + new Vector3(0, height, -distance);

		transform.position = Vector3.Lerp(transform.position, positionTarget, Time.deltaTime * slerp);

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, FieldOfView, Time.deltaTime * slerp);
    }

	private void SetHeight()
	{
		if (OverrideHeight)
		{
			height = Mathf.Lerp(height, OverriddenHeight, Time.deltaTime * slerp);
		}
		else if (SlerpBack)
		{
			height = Mathf.Lerp(height, Height, Time.deltaTime * slerp);
		}
		else
		{
			height = Height;
		}
	}
	private void SetDistance()
	{
		if (OverrideDistance)
		{
			distance = Mathf.Lerp(distance, OverriddenDistance, Time.deltaTime * slerp);
		}
		else if (SlerpBack)
		{
			distance = Mathf.Lerp(distance, Distance, Time.deltaTime * slerp);
		}
		else
		{
			distance = Distance;
		}
	}
	private void SetSlerp()
	{
		if (OverrideSlerp || OverrideRotationSlerp)
		{
            slerp = OverrideSlerp ? OverriddenSlerp : MoveSlerpAmount;
            rotationSlerp = OverrideRotationSlerp ? OverriddenRotationSlerp : RotationSlerpAmount;
        }
        else if(SlerpBack)
        {
            slerp = OverriddenSlerp > 0f ? OverriddenSlerp : MoveSlerpAmount;
            rotationSlerp = OverriddenRotationSlerp > 0f ? OverriddenRotationSlerp : RotationSlerpAmount;
        }
		else
		{
			slerp = MoveSlerpAmount;
            rotationSlerp = RotationSlerpAmount;
        }
	}

	void TransformLook()
	{
		Vector3 lookTarget = player.transform.position;
		Vector3 relativePos = lookTarget - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos + new Vector3(0, XRotationOffset, 0));
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSlerp);
	}
}
