using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TargetEffect : MonoBehaviour
{
    public GameObject Effect;

    public void SetTarget()
    {
        var effect = (GameObject)Instantiate(Effect);
        effect.transform.SetParent(this.transform);

        effect.transform.position = this.transform.position + new Vector3(0,1f,0);
    }
}
