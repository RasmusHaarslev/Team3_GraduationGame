using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	GameObject player;

    #region Inspector fields
    [Tooltip("Sets the distance away from the player")] public float Distance = 15f;
    [Tooltip("Sets the height relative to the player")] public float Height = 8f;
    [Tooltip("Sets amount of slerp (higher = faster)")] public float SlerpAmount = 1f;
    [Tooltip("Sets offset for the camera look direction, relative to the players direction")] public float CameraOffset = 1f;
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

    #endregion


    // Use this for initialization
    void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
	    TransformPosition();
        TransformLook();
    }

    void TransformPosition()
    {
        float height = OverrideHeight ? OverriddenHeight : Height;
        float distance = OverrideDistance ? OverriddenDistance : Distance;
        float slerp = OverrideSlerp ? OverriddenSlerp : SlerpAmount;

        Vector3 clickedTarget = player.GetComponent<NavMeshAgent>().destination;
        Vector3 offset = player.transform.forward.normalized;

        Vector3 positionTarget = OverridePosition ? OverriddenPosition : player.transform.position + (new Vector3(0, height, -distance) + offset * 2f);
        
        transform.position = Vector3.Lerp(transform.position, positionTarget, Time.deltaTime * SlerpAmount);
    }

    void TransformLook()
    {
        Vector3 offset = player.transform.forward.normalized * CameraOffset;
        Vector3 lookTarget = player.transform.position + offset;
        transform.LookAt(lookTarget);
    }
}
