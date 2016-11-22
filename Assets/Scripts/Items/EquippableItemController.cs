using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class ItemController
{
    public static List<ItemXML> ItemsLoaded = new List<ItemXML>();

    public static void SaveCharacters(List<ItemXML> characters)
    {
        var itemsXML = new ItemsXML();

        foreach (ItemXML entry in characters)
        {
            ItemsLoaded.Add(entry);
            itemsXML.Items.Add(entry);
        }

        var path = Path.Combine(PersistentData.GetPath(), "characters.xml");

        var serializer = new XmlSerializer(typeof(CharactersXML));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, itemsXML);
        stream.Close();
    }

    public static List<ItemXML> LoadCharacters()
    {
        var path = Path.Combine(PersistentData.GetPath(), "characters.xml");

        if (!File.Exists(path))
        {
            Debug.LogError("No levels generated, reset game");
            return new List<ItemXML>();
        }

        var serializer = new XmlSerializer(typeof(ItemsXML));
        var stream = new FileStream(path, FileMode.Open);
        var container = serializer.Deserialize(stream) as ItemsXML;
        stream.Close();

        ItemsLoaded = new List<ItemXML>();

        foreach (ItemXML character in container.Items)
        {
            ItemsLoaded.Add(character);
        }

        return ItemsLoaded;
    }
}

public class ItemsXML
{
    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<ItemXML> Items = new List<ItemXML>();
}

public class ItemXML
{
    public int id { get; set; }
    public string name { get; set; }
    public type Type { get; set; }
    public slot Slot { get; set; }
    public int characterId { get; set; }
    public string rarity { get; set; }
    public int level { get; set; }
    public string description { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public float damageSpeed { get; set; }
    public int range { get; set; }
    public string prefabName { get; set; }
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