using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour {

	public float timer = 1f;

	void Update () {
		transform.LookAt(Camera.main.transform.position, -Vector3.up);
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			Destroy(gameObject);
		}
	}
}
