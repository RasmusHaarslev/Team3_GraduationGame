
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

public class EnemySpottedEvent : GameEvent {

	public Vector3 pos;

	public EnemySpottedEvent(Vector3 pos) 
	{
		this.pos = pos;
	}

}

public class CeaseFightingEvent : GameEvent {

	public CeaseFightingEvent() 
	{
	}

}

public class AttackStateEvent : GameEvent
{
    public AttackStateEvent()
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