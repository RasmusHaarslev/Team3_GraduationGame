using UnityEngine;
using System.Collections;

public class RotateWeapon : MonoBehaviour {

    void Update()
    {

        if (gameObject.name.ToString() == "Rifle")
        {
            transform.Rotate(0, 0, 50 * Time.deltaTime);
        }
        if (gameObject.name.ToString() == "Shield" || gameObject.name.ToString() == "Shield2")
        {
            transform.Rotate(0, 50 * Time.deltaTime, 0);
        }
        if (gameObject.name.ToString() == "Stick")
        {
            transform.Rotate(0, 50 * Time.deltaTime, 0);
        }
        
    }
}
