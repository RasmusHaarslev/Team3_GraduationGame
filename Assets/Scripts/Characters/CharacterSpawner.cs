using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
//[ExecuteInEditMode]
public class CharacterSpawner : MonoBehaviour
{
    
    public string characterName;
<<<<<<< HEAD
    public Color GizmoColor = Color.red;
    [Range(1.0f, 5.0f)]
    public float gizmoSize = 1.0f;
    public int tier = 1;
    /**/
    void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoSize * 0.3f);
        Handles.color = GizmoColor;
        //Handles.SphereCap(0, transform.position, transform.rotation, gizmoSize*0.5f);
        Handles.ArrowCap(0, transform.position, transform.rotation, gizmoSize*1.5f);
        //Handles.ConeCap(0, transform.position, transform.rotation, 0.2f);
    }
=======
 //   public Color GizmoColor = Color.red;
 //   [Range(1.0f, 5.0f)]
 //   public float gizmoSize = 1.0f;

 //   /**/
 //   void OnDrawGizmos()
 //   {
 //       Gizmos.color = GizmoColor;
 //       Gizmos.DrawSphere(transform.position, gizmoSize * 0.3f);
 //       Handles.color = GizmoColor;
 //       //Handles.SphereCap(0, transform.position, transform.rotation, gizmoSize*0.5f);
 //       Handles.ArrowCap(0, transform.position, transform.rotation, gizmoSize*1.5f);
	//	//Handles.ConeCap(0, transform.position, transform.rotation, 0.2f);
	//}
>>>>>>> 8c5e29ac0e3a4292ba527a136a6d1d4dee359f78
    

 //   // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    
}

