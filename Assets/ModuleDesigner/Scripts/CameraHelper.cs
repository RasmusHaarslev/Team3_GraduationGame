using System;
using UnityEngine;
using System.Collections;

public class CameraHelper : MonoBehaviour {

    public GameObject Target;

    #region Inspector fields
    [Tooltip("Sets the distance away from the player")]
    public float Distance = 15f;
    [Tooltip("Sets the height relative to the player")]
    public float Height = 8f;
    [Tooltip("Sets amount of slerp (higher = faster)")]
    public float SlerpAmount = 1f;
    [Tooltip("Sets offset for the camera look direction, relative to the players direction")]
    public float CameraOffset = 1f;
    #endregion

    #region Hidden public fields
    public Boolean OverridePosition = false;
    public Vector3 OverriddenPosition = new Vector3();

    public Boolean OverrideDistance = false;
    public float OverriddenDistance = 0f;

    public Boolean OverrideHeight = false;
    public float OverriddenHeight = 0f;

    #endregion

    void Start()
    {
        Destroy(this.gameObject);
    }

    public void Update()
    {
        if(Target != null) { 
            TransformPosition();
            TransformLook();
        }
    }

    void TransformPosition()
    {
        float height = OverrideHeight ? OverriddenHeight : Height;
        float distance = OverrideDistance ? OverriddenDistance : Distance;

        Vector3 positionTarget = OverridePosition ? OverriddenPosition : Target.transform.position + (new Vector3(0, height, -distance) * 2f);

        transform.position = positionTarget;
    }

    void TransformLook()
    {
        Vector3 offset = Target.transform.forward.normalized;
        Vector3 lookTarget = Target.transform.position + offset * 2f;
        transform.LookAt(lookTarget);
    }

}
