using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour {

	// Use this for initialization
	public void StartCutscene () {
        ((MovieTexture)GetComponent<RawImage>().mainTexture).Play();
    }
}
