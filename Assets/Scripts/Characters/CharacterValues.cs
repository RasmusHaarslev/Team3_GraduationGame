using UnityEngine;
using System.Collections;
using SQLite;

/// <summary>
/// Contains all the character saved values from the database.
/// </summary>
public class CharacterValues
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string name { get; set; }
    public type Type { get; set; }
    public string description { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public int damageSpeed { get; set; }
    public int range { get; set; }
	public int tier { get; set; }

    public combatFocusType CombatFocusType { get; set; }
    public combatFleeType CombatFleeType { get; set; }
    public outOfCombatMovementType OutOfCombatMovementType { get; set; }

	public enum type
	{
		Tribesman,
		Wolf,
		Hunter,
		Player
	}


	public enum combatFocusType 
    {
        PlayerAttackers,
        PlayerFocus,
        BossTier,
        HighTier,
        MidTier,
        LowTier,
        Nearest,
        Defensive
    };

public enum combatFleeType
    {
        Default,
        Brave,
        Fearful
    };

    public enum outOfCombatMovementType
    {
        StandStill,
        FollowLeader,
        Patrolling
    }
    public string prefabName { get; set; }
    public override string ToString()
    {
        return string.Format("[CharacterInfo: Id={0}, Name={1},  prefabName={2}]", id, name, prefabName);
    }
}
