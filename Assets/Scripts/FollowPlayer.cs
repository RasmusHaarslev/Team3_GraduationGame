using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	GameObject player;
    public Vector3 CameraPositionInRelationToPlayer = new Vector3(0, 12, -13);

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position + CameraPositionInRelationToPlayer;
	}
}
