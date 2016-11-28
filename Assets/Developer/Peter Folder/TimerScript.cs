﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    public Text text;
    public CampManager campManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        text.text = campManager.TimeLeftInSeconds()+""; 
	}
}
