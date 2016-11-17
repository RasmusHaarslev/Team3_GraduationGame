
using UnityEngine;

public class SaveLevelsToXML : GameEvent
{
    public SaveLevelsToXML () { }
}

public class ChangeResources : GameEvent
{
    public int food;
    public int coins;
    public int villager;

    public ChangeResources(int food = 0, int coins = 0, int villager = 0)
    {
        this.food = food;
        this.coins = coins;
        this.villager = villager;
    }
}

public class ResourcesUpdated : GameEvent
{
    public ResourcesUpdated() { }
}

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

public class LevelWon : GameEvent
{
    public LevelWon()
    {

    }
}

public class PositionClicked : GameEvent
{
    public Vector3 position;
    public PositionClicked(Vector3 clickPosition) {
        position = clickPosition;
    }

}


public class LevelLost : GameEvent
{
    public LevelLost()
    {

    }
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
	public GameObject enemy;
	public EnemyDeathEvent(GameObject enemy)
	{
		this.enemy = enemy;
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

public class ItemSpawned : GameEvent
{
    public ItemSpawned()
    {
        
    }
}

public class EnemySpawned : GameEvent
{
    private CharacterValues enemyValues;
    public EnemySpawned(CharacterValues values)
    {
        enemyValues = values;
    }
}

public class PlayerDeathEvent : GameEvent
{
	public PlayerDeathEvent()
	{
	}
}