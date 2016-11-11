using UnityEngine;
using System.Collections;

public class Row : MonoBehaviour {

    GameObject ScrollView;

	// Use this for initialization
	void Start () {

        ScrollView = GameObject.FindGameObjectWithTag("ScrollView");

        BoxCollider2D scrollviewCollider = ScrollView.GetComponent<BoxCollider2D>();

        if (!GetComponent<BoxCollider2D>().bounds.Intersects(scrollviewCollider.bounds))
        {
           // gameObject.SetActive(false);
        }
    }	
}
