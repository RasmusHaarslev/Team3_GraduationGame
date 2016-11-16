using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterSpawner : MonoBehaviour
{

    //public string characterName;
    [Range(1,3)]
    public int tier = 1;
    private Color GizmoColor = Color.red;
    private float gizmoSize = 1.0f;
    
    /**/
    void OnDrawGizmos()
    {
        GizmoColor = new Color(1f, (3 - tier) * 0.5f, 0f, 1f);
        gizmoSize = 0.5f + tier * 0.5f;

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

