
using UnityEngine;

public class SetupPopUp : GameEvent
{
    public GameObject node;

    public SetupPopUp(GameObject node)
    {
        this.node = node;
    }
}

public class TakeDamageEvent : GameEvent
{
    public float damage { get; private set; }
	public GameObject target;

    public TakeDamageEvent(float damage, GameObject target)
    {
        this.damage = damage;
		this.target = target;
    }
}

public class InstantiateGame : GameEvent {

	public InstantiateGame () {}

}

public class EnemySpottedEvent : GameEvent {

	public GameObject parent;

	public EnemySpottedEvent(GameObject parent) 
	{
		this.parent = parent;
	}

}

public class CeaseFightingEvent : GameEvent {

	public CeaseFightingEvent() 
	{
	}

}

public class OffensiveStateEvent : GameEvent
{
    public OffensiveStateEvent()
    {
    }
}


public class DefendStateEvent : GameEvent
{
    public DefendStateEvent()
    {
    }
}

public class EnemyDeathEvent : GameEvent
{
	public EnemyDeathEvent()
	{

	}

}

public class StayStateEvent : GameEvent
{
	public StayStateEvent()
	{

	}
}

public class FleeStateEvent : GameEvent
{
	public FleeStateEvent()
	{

	}
}

public class FollowStateEvent : GameEvent
{
	public FollowStateEvent()
	{

	}
}

public class AllyDeathEvent : GameEvent
{
	public AllyDeathEvent()
	{

	}
}