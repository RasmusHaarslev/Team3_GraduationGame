using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ResetTimescale : MonoBehaviour
{
    void Awake() {
        Time.timeScale = 1f;
    }
}