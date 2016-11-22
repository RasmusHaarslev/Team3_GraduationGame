using UnityEngine;
using System.Collections;

public class RotateCircle : MonoBehaviour {

    void Update()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }
}
