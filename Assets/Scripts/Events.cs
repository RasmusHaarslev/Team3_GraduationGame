
using UnityEngine;

public class TakeDamageEvent : GameEvent
{
    public float damage { get; private set; }

    public TakeDamageEvent(float damage)
    {
        this.damage = damage;
    }
}

public class InstantiateGame : GameEvent {

	public InstantiateGame () {}

}


// The event called when the attack command is cast
public class OffensiveStateEvent : GameEvent
{
    public OffensiveStateEvent()
    {
    }
}

// The event called when the defend command is cast
public class DefendStateEvent : GameEvent
{
    public DefendStateEvent()
    {
    }
}

// The event called when the flee command is cast
public class FleeStateEvent : GameEvent
{
	public FleeStateEvent()
	{
	}
}

// The event called when the follow command is cast
public class FollowStateEvent : GameEvent
{
	public FollowStateEvent()
	{
	}
}

// The event called when the stay command is cast
public class StayStateEvent : GameEvent
{
	public StayStateEvent()
	{
	}
}

#region Prototype events
// Events required for functional prototype
public class EnemyDeathEvent : GameEvent
{
	public EnemyDeathEvent()
	{

	}

}


public class EnemySpottedEvent : GameEvent
{

	public Vector3 pos;

	public EnemySpottedEvent(Vector3 pos)
	{
		this.pos = pos;
	}

}

public class CeaseFightingEvent : GameEvent
{

	public CeaseFightingEvent()
	{
	}

}
#endregion