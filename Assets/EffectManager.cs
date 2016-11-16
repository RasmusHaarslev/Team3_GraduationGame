using UnityEngine;
using System.Collections;
using System;

public class EffectManager : MonoBehaviour {

    public GameObject clicking;
    private GameObject _currentClick;

    // Use this for initialization
    void OnEnable () {
        EventManager.Instance.StartListening<PositionClicked>(positionEffect);
	}

    private void positionEffect(PositionClicked e)
    {
        if (_currentClick) {
            var ps = _currentClick.GetComponent<ParticleSystem>();
            ps.playbackSpeed = 4f;
            ps.Stop();
        }

        _currentClick = (GameObject)Instantiate(clicking, e.position, Quaternion.Euler(90f, 0f, 0f));
    }

    // Update is called once per frame
    void OnDisable() {
	    
	}
}
