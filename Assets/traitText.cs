using UnityEngine;
using System.Collections;

public class traitText : MonoBehaviour {

	public string trait = "";
	public float timer = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnEnable()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		transform.eulerAngles = new Vector3(90, 0, 0);
		GetComponent<TextMesh>().text = trait;
	}
}
