using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
//[ExecuteInEditMode]
public class CharacterSpawner : MonoBehaviour
{
    public string characterName;
    public Color GizmoColor = Color.red;
    [Range(1.0f, 5.0f)]
    public float gizmoSize = 1.0f;

    /**/
    void OnDrawGizmos()
    {
    	#if UNITY_EDITOR
	Gizmos.color = GizmoColor;
	Gizmos.DrawSphere(transform.position, gizmoSize * 0.3f);
	Handles.color = GizmoColor;
	//Handles.SphereCap(0, transform.position, transform.rotation, gizmoSize*0.5f);
	Handles.ArrowCap(0, transform.position, transform.rotation, gizmoSize*1.5f);
	//Handles.ConeCap(0, transform.position, transform.rotation, 0.2f);
	#endif

    }
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    
}
