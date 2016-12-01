using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class ItemController
{
    public static List<EquippableitemValues> ItemsLoaded = new List<EquippableitemValues>();

    public static void SaveItem(List<EquippableitemValues> items)
    {
        var itemsXML = new ItemsXML();

        foreach (EquippableitemValues entry in ItemsLoaded)
        {
            itemsXML.Items.Add(entry);
        }
        
        //foreach (EquippableitemValues item in items)
        //{
        //    ItemsLoaded.Add(item);
        //    itemsXML.Items.Add(item);
        //}

        var path = Path.Combine(PersistentData.GetPath(), "items.xml");

        var serializer = new XmlSerializer(typeof(ItemsXML));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, itemsXML);
        stream.Close();
    }

    public static List<EquippableitemValues> LoadItems()
    {
        var path = Path.Combine(PersistentData.GetPath(), "items.xml");

        if (!File.Exists(path))
        {
            Debug.LogError("No items generated, reset game");
            return ItemsLoaded;
        }

        var serializer = new XmlSerializer(typeof(ItemsXML));
        var stream = new FileStream(path, FileMode.Open);
        var container = serializer.Deserialize(stream) as ItemsXML;
        stream.Close();

        ItemsLoaded = new List<EquippableitemValues>();

        foreach (EquippableitemValues item in container.Items)
        {
            ItemsLoaded.Add(item);
        }

        return ItemsLoaded;
    }
}

public class ItemsXML
{
    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<EquippableitemValues> Items = new List<EquippableitemValues>();
}