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
        //_currentClick.GetComponent<ParticleSystem>().Stop();
        if (_currentClick == null)
            InitLight();

        _currentClick.transform.position = e.position + new Vector3(0,0.5f,0);
        //_currentClick.GetComponent<ParticleSystem>().Play();
    }

    void InitLight()
    {
        _currentClick = (GameObject)Instantiate(clicking, new Vector3(), Quaternion.Euler(90f, 0f, 0f));
    }

    // Update is called once per frame
    void OnDisable() {
	    
	}
}
