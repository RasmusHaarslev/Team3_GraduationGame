﻿
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

public class PlayerSpottedEvent : GameEvent {

	public Vector3 lastSeenPosition { get; private set; }

	public PlayerSpottedEvent(Vector3 lastSeenPosition) {
		this.lastSeenPosition = lastSeenPosition;
	}

}

public class AlarmEvent : GameEvent {

	public Vector3 AlarmAtPosition { get; private set; }

	public AlarmEvent(Vector3 AlarmAtPosition) {
		this.AlarmAtPosition = AlarmAtPosition;
	}

}
 
public class DeadEvent : GameEvent {

	public DeadEvent() {
	}

}

public class PickUpKey : GameEvent {
    public GameObject key;

    public PickUpKey (GameObject key) {
        this.key = key;
    }
}

public class OpenGate : GameEvent {

	public OpenGate () {
	}
}

public class CompletedLevel : GameEvent {

    public CompletedLevel () {
    }
}

public class StartTimer : GameEvent {

    public float Time;

    public StartTimer(float Time) {
        this.Time = Time;
    }
}

public class RemoveUI : GameEvent {

    public int amountToRemove;

    public RemoveUI(int amountToRemove) {
        this.amountToRemove = amountToRemove;
    }
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