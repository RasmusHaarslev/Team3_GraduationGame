using UnityEngine;
using System.Collections;

public class ToggleObjectOnPlayerTrigger : MonoBehaviour {
	public GameObject objectToToggle;
	// Use this for initialization
	void OnTriggerEnter (Collider c) {
		if(c.CompareTag("Player"))
		{
			objectToToggle.SetActive (true);
		}
	}
	
	void OnTriggerExit (Collider c) {
		if(c.CompareTag("Player"))
		{
			objectToToggle.SetActive (false);
		}
	}
}
