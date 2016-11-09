using UnityEngine;
using System.Collections;

public class AttenuationSpheres : MonoBehaviour {

    [Header("Gizmo options")]
    public bool ShowGizmo = true;
    [Range(0.0f,1.0f)]
    public float ColorAlpha = 0.5f;

    [Header("Inner circle")]
    public float InnerCircle = 5f;
    public Color InnerCircleColor = Color.red;

    [Header("Outer circle")]
    public float OuterCircle = 10f;
    public Color OuterCircleColor = Color.green;

    void OnDrawGizmos()
    {
        if (ShowGizmo) { 
            Gizmos.color = new Color(InnerCircleColor.r, InnerCircleColor.g, InnerCircleColor.b, ColorAlpha);
            Gizmos.DrawSphere(transform.position, InnerCircle);

            Gizmos.color = new Color(OuterCircleColor.r, OuterCircleColor.g, OuterCircleColor.b, ColorAlpha);
            Gizmos.DrawSphere(transform.position, OuterCircle);
        }
    }
}
