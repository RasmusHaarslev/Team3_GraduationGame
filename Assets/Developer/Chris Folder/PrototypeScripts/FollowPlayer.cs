using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	GameObject player;

    [Tooltip("Sets the distance away from the player")]
    public float Distance = 15f;
    [Tooltip("Sets the height relative to the player")]
    public float Height = 8f;
    [Tooltip("Sets amount of slerp (higher = faster)")]
    public float SlerpAmount = 1f;
    [Tooltip("Sets offset for the camera look direction, relative to the players direction")]
    public float CameraOffset = 1f;

    // Use this for initialization
    void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Vector3 clickedTarget = player.GetComponent<NavMeshAgent>().destination;
        Vector3 offset = player.transform.forward.normalized;

        Vector3 positionTarget = player.transform.position + (new Vector3(0, Height, -Distance) + offset * 2f);
        Vector3 lookTarget = player.transform.position + offset * 2f;

        transform.position = Vector3.Lerp(transform.position, positionTarget, Time.deltaTime * SlerpAmount);
        transform.LookAt(lookTarget);
    }
}
