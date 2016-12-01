using UnityEngine;
using System.Collections;
using SQLite;
using System.Xml.Serialization;

/// <summary>
/// Contains all the character saved values from the database.
/// </summary>
public class CharacterValues
{
	[PrimaryKey, AutoIncrement, XmlAttribute("id")]
	public int id { get; set; }

    [XmlAttribute("name")]
    public string name { get; set; }

    [XmlAttribute("isMale")]
    public bool isMale { get; set; }

    [XmlAttribute("Type")]
    public type Type { get; set; }

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

    [XmlAttribute("tier")]
    public int tier { get; set; }

    [XmlAttribute("targetTrait")]
    public TargetTrait targetTrait { get; set; }

    [XmlAttribute("combatTrait")]
    public CombatTrait combatTrait { get; set; }

	public enum type
	{
		Tribesman,
		Wolf,
		Hunter,
		Player,
        NewHunter
	}

	public enum TargetTrait
	{
		NoTrait,
		Codependant,
		LowAttentionSpan,
		Loyal
	}
	public enum CombatTrait
	{
		NoTrait,
		BraveFool,
		Fearful,
		Clingy
	}

    [XmlAttribute("prefabName")]
    public string prefabName { get; set; }

    [XmlAttribute("materialName")]
    public string materialName { get; set; }

    public override string ToString()
	{
		return string.Format("[CharacterInfo: Id={0}, Name={1},  prefabName={2}]", id, name, prefabName);
	}
}
