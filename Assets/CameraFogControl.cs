using UnityEngine;
using System.Collections;

public class CameraFogControl : MonoBehaviour {
	public UnityStandardAssets.ImageEffects.GlobalFogExtended fog;
	// Use this for initialization
	void Start () {
		CollectLevelLight ();
	}
	
	void CollectLevelLight()
	{
		GameObject b =  GameObject.FindGameObjectWithTag ("LevelLight");
		if(b != null)
		{
			fog.Advanced.sun = b; 
		}
	}
}
