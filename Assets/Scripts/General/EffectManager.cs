using UnityEngine;
using System.Collections;
using System;

public class EffectManager : MonoBehaviour {

    public GameObject Clicking;
    private GameObject _currentClick;
    public GameObject Target;
    private GameObject _currentTarget;

    // Use this for initialization
    void OnEnable () {
        EventManager.Instance.StartListening<PositionClicked>(positionEffect);
        EventManager.Instance.StartListening<EnemyClicked>(setTarget);
        EventManager.Instance.StartListening<EnemyDeathEvent>(checkTarget);
    }

    private void checkTarget(EnemyDeathEvent e)
    {
        //if (e.enemy.transform == _currentTarget.transform.parent)
        //{
        //    _currentTarget.GetComponent<ParticleSystem>().Stop();
        //}
    }

    private void positionEffect(PositionClicked e)
    {
        if (_currentTarget != null)
            _currentTarget.GetComponent<ParticleSystem>().Stop();
        if (_currentClick != null)
            _currentClick.GetComponent<ParticleSystem>().Stop();

        if (_currentClick == null)
            if (e.hitted.gameObject.name != "Ground")
            {
                _currentClick = (GameObject)Instantiate(Clicking);
            }

        if (e.hitted.gameObject.name != "Ground") {             
            _currentClick.transform.position = e.position + new Vector3(0, 0.5f, 0);
            _currentClick.GetComponent<ParticleSystem>().Play();
        }
    }

    private void setTarget(EnemyClicked e)
    {
        if (_currentTarget != null)
            _currentTarget.GetComponent<ParticleSystem>().Stop();
        if (_currentClick != null)
            _currentClick.GetComponent<ParticleSystem>().Stop();

        if (_currentTarget == null)
            _currentTarget = (GameObject)Instantiate(Target);

        _currentTarget.transform.SetParent(e.enemy.transform);
        _currentTarget.transform.position = e.enemy.transform.position + new Vector3(0, 1f, 0);
        _currentTarget.GetComponent<ParticleSystem>().Play();
    }
}
