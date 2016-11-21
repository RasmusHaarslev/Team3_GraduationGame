using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ClickingEffect : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Play();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 0.1f);
        if (!_particleSystem.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}

