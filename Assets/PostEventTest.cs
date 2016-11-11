using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PostEventTest : MonoBehaviour {
	public string eventName;
	public List<Transform> sourcePositions;
	AkPositionArray posArray;
	// Use this for initialization
	void Start () 
	{
		SetUpMultiPositions ();

		if(string.IsNullOrEmpty(eventName))
		{
			Manager_Audio.PlaySound (eventName,this.gameObject);
		}
	}

	void SetUpMultiPositions()
	{
		posArray = new AkPositionArray((ushort)sourcePositions.Count);

		foreach(var v in sourcePositions)
		{
			posArray.Add (v.position, v.forward,v.up*1);
		}

		Manager_Audio.MultiPosition (this.gameObject, posArray);
	}

}
