using UnityEngine;
using System.Collections;

public class CameraFogControl : MonoBehaviour {
	public UnityStandardAssets.ImageEffects.GlobalFogExtended fog;
	// Use this for initialization
	void Start () {
		fog.Advanced.sun = GameObject.FindGameObjectWithTag ("LevelLight");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
