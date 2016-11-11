using UnityEngine;
using System.Collections;

public class FellowshipSpawnPoint : MonoBehaviour {

    public Color GizmoColor = Color.cyan;
    [Range(1.0f, 5.0f)]
    public float gizmoSize = 1.0f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       
    }

    void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoSize*0.3f);
    }

}
