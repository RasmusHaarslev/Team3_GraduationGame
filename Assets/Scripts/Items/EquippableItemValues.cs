using UnityEngine;
using System.Collections;
using SQLite;
using System.Xml.Serialization;

public class EquippableitemValues
{
    [PrimaryKey, AutoIncrement, XmlAttribute("id")]
    public int id { get; set; }

    [XmlAttribute("name")]
    public string name { get; set; }

    [XmlAttribute("Type")]
    public type Type { get; set; }

    [XmlAttribute("Slot")]
    public slot Slot { get; set; }

    [XmlAttribute("characterId")]
    public int characterId { get; set; }

    [XmlAttribute("rarity")]
    public string rarity { get; set; }

    [XmlAttribute("level")]
    public int level { get; set; }

    [XmlAttribute("description")]
    public string description { get; set; }

    [XmlAttribute("damage")]
    public int damage { get; set; }

    [XmlAttribute("health")]
    public int health { get; set; }

    [XmlAttribute("damageSpeed")]
    public float damageSpeed { get; set; }

    [XmlAttribute("range")]
    public int range { get; set; }

    [XmlAttribute("prefabName")]
    public string prefabName { get; set; }

    [XmlAttribute("materialName")]
    public string materialName { get; set; }

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