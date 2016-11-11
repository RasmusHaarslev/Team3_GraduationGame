using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterSpawner : MonoBehaviour
{

    //public string characterName;
    public int tier = 1;
    public Color GizmoColor = Color.red;
    [Range(1.0f, 5.0f)]
    public float gizmoSize = 1.0f;
    
    /**/
    void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoSize * 0.3f);
        #if UNITY_EDITOR
        Handles.color = GizmoColor;
        Handles.ArrowCap(0, transform.position, transform.rotation, gizmoSize*1.5f);
        #endif
    }



    //   // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    
}

