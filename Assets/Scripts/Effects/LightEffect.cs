using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
    private GameObject _player;
    private Light _light;

    void Start()
    {
        //_player = GameObject.Find("Player(Clone)");
        _light = this.GetComponent<Light>();
    }

    void Update()
    {
        var dist = Vector3.Distance(this.transform.position, Camera.main.GetComponent<CameraController>().player.transform.position);
        _light.intensity = 4f - (5f / dist);
    }
}