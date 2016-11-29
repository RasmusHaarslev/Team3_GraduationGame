using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterController
{
    public static List<CharacterXML> CharactersLoaded = new List<CharacterXML>();

    public static void SaveCharacters(List<CharacterXML> characters)
    {
        var charactersXML = new CharactersXML();

        foreach (CharacterXML entry in characters)
        {
            CharactersLoaded.Add(entry);
            charactersXML.Characters.Add(entry);
        }

        var path = Path.Combine(PersistentData.GetPath(), "characters.xml");

        var serializer = new XmlSerializer(typeof(CharactersXML));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, charactersXML);
        stream.Close();
    }

    public static List<CharacterXML> LoadCharacters()
    {
        var path = Path.Combine(PersistentData.GetPath(), "characters.xml");

        if (!File.Exists(path))
        {
            Debug.LogError("No levels generated, reset game");
            return new List<CharacterXML>();
        }

        var serializer = new XmlSerializer(typeof(CharactersXML));
        var stream = new FileStream(path, FileMode.Open);
        var container = serializer.Deserialize(stream) as CharactersXML;
        stream.Close();

        CharactersLoaded = new List<CharacterXML>();

        foreach (CharacterXML character in container.Characters)
        {
            CharactersLoaded.Add(character);
        }

        return CharactersLoaded;
    }
}

public class CharactersXML
{
    [XmlArray("Characters")]
    [XmlArrayItem("Character")]
    public List<CharacterXML> Characters = new List<CharacterXML>();
}

public class CharacterXML
{
    public int id { get; set; }
    public string name { get; set; }
    public bool isMale { get; set; }
    public type Type { get; set; }
    public string description { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public float damageSpeed { get; set; }
    public int range { get; set; }
    public int tier { get; set; }

    public TargetTrait targetTrait { get; set; }
    public CombatTrait combatTrait { get; set; }

    public enum type
    {
        Tribesman,
        Wolf,
        Hunter,
        Player
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

    public string prefabName { get; set; }
    public string materialName { get; set; }

    public override string ToString()
    {
        return string.Format("[CharacterInfo: Id={0}, Name={1},  prefabName={2}]", id, name, prefabName);
    }
}