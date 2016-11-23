using UnityEngine;
using System.Collections;

public class traitText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnEnable()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		GetComponent<TextMesh>().text = transform.parent.GetComponent<Character>().characterBaseValues.combatTrait + " + " + transform.parent.GetComponent<Character>().characterBaseValues.targetTrait;
	}
}
