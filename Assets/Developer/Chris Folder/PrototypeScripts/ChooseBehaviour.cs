using UnityEngine;
using System.Collections;

public class ChooseBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack()
    {
        EventManager.Instance.TriggerEvent(new OffensiveStateEvent());
    }

    public void Defend()
    {
        EventManager.Instance.TriggerEvent(new DefendStateEvent());
    }

	public void Flee()
	{
		EventManager.Instance.TriggerEvent(new FleeStateEvent());
	}

	public void Stay()
	{
		EventManager.Instance.TriggerEvent(new StayStateEvent());
	}

	public void Follow()
	{
		EventManager.Instance.TriggerEvent(new FollowStateEvent());
	}
}
