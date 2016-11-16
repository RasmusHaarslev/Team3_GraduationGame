using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ClickingEffect : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    void Update()
    {
        if (!particleSystem.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}

