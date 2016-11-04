using UnityEngine;
using System.Collections;

public class TriggerChase : MonoBehaviour {

	GameObject parentGo;

	// Use this for initialization
	void Start () {
		parentGo = transform.root.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Friendly" ) {
			parentGo.GetComponent<EnemyAI> ().chaseFriendly (col.transform.position);
		}
	}
}
