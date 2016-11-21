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
        //_particleSystem.Play();
    }
}

