using UnityEngine;
using System.Collections;
using SQLite;
public class InventoryItem
{

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public type Type { get; set; }
    public int characterId { get; set; }
    public int quantity { get; set; }
    
    public enum type
    {
        equippable,
        collectable,
        consumable
    }
    public override string ToString()
    {
        return string.Format("[EquippableitemValues: Id={0}, Type={1},  quantity={2}]", id, Type, quantity);
    }



}