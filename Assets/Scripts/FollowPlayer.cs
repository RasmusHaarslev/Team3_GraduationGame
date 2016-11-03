using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	GameObject player;
	private Vector3 positionPrev; //stored position of the player, from last step.
    public Vector3 CameraPositionInRelationToPlayer = new Vector3(0, 12, -13);
	public float CameraMoveOffset = 1.0f;
	public float CameraMoveSnappyness = 0.015f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		positionPrev = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerRelativeMovement = (player.transform.position - positionPrev) / Time.deltaTime;
		Vector3 targetCamPosition = player.transform.position + CameraPositionInRelationToPlayer + new Vector3(playerRelativeMovement.x,0,playerRelativeMovement.z) *CameraMoveOffset;

		positionPrev = player.transform.position;

		transform.position += (targetCamPosition - transform.position) * 0.015f;
	}
}
