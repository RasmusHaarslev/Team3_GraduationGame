using UnityEngine;
using System.Collections;
using SQLite;
public class EquippableitemValues
{

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string name { get; set; }
    public type Type { get; set; }
    public slot Slot { get; set; }
    public int characterId { get; set; }
    public string rarity { get; set; }
    public string description { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public float damageSpeed { get; set; }
    public int range { get; set; }
    public string prefabName { get; set; }

    public enum slot
    {
        head,
        torso,
        leftHand,
        rightHand,
        legs
    }
    public enum type
    {
        polearm,
        shield,
        rifle
    }
    public override string ToString()
    {
        return string.Format("[EquippableitemValues: Id={0}, Name={1},  prefabName={2}]", id, name, prefabName);
    }



}