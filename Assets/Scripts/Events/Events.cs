
using UnityEngine;

public class SaveLevelsToXML : GameEvent
{
    public SaveLevelsToXML () { }
}

public class LevelCleared : GameEvent
{
    public bool isCleared;

    public LevelCleared(bool isCleared)
    {
        this.isCleared = isCleared;
    }
}

public class ChangeResources : GameEvent
{
    public int food;
    public int scraps;
    public int villager;
    public int premium;
    public int daysSurvived;

    public ChangeResources(int food = 0, int scraps = 0, int villager = 0, int premium = 0, int daysSurvived = 0)
    {
        this.food = food;
        this.scraps = scraps;
        this.villager = villager;
        this.premium = premium;
        this.daysSurvived = daysSurvived;
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
    public Transform hitted;

    public PositionClicked(Vector3 clickPosition, Transform hitted) {
        position = clickPosition;
        this.hitted = hitted;
    }
}

public class EnemyClicked : GameEvent
{
    public GameObject enemy;

    public EnemyClicked(GameObject enemy)
    {
        this.enemy = enemy;
    }
}

public class ItemClicked : GameEvent
{
    public ClickableItem item;
    public ItemClicked(ClickableItem clickedItem)
    {
        item = clickedItem;
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

public class EnemyAttackedByLeaderEvent : GameEvent
{
	public GameObject enemy;
	public EnemyAttackedByLeaderEvent(GameObject enemy)
	{
		this.enemy = enemy;
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
    public Character deadAlly;

    public AllyDeathEvent()
	{

	}
    public AllyDeathEvent(Character deadAlly)
    {
        this.deadAlly = deadAlly;
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

public class ChangeFormationEvent : GameEvent
{
	public GameObject hunter;

	public ChangeFormationEvent(GameObject hunter)
	{
		this.hunter = hunter;
	}
}

public class CommandEvent : GameEvent
{
	public CommandEvent()
	{

	}
}

public class ClearedCampEvent : GameEvent
{
	public ClearedCampEvent()
	{

	}
}

public class LanguageChanged : GameEvent
{
    public LanguageChanged() { }
}

public class StopFleeEvent : GameEvent
{
	public StopFleeEvent()
	{

	}
}