using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterController
{
    public static List<CharacterValues> CharactersLoaded = new List<CharacterValues>();

    public static void SaveCharacters(List<CharacterValues> characters)
    {
        var charactersXML = new CharactersXML();

        foreach (CharacterValues entry in CharactersLoaded)
        {
            charactersXML.Characters.Add(entry);
        }

        //foreach (CharacterValues entry in characters)
        //{
        //    CharactersLoaded.Add(entry);
        //    charactersXML.Characters.Add(entry);
        //}

        var path = Path.Combine(PersistentData.GetPath(), "characters.xml");

        var serializer = new XmlSerializer(typeof(CharactersXML));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, charactersXML);
        stream.Close();
    }

    public static List<CharacterValues> LoadCharacters()
    {
        var path = Path.Combine(PersistentData.GetPath(), "characters.xml");

        if (!File.Exists(path))
        {
            Debug.LogError("No levels generated, reset game");
            return CharactersLoaded;
        }

        var serializer = new XmlSerializer(typeof(CharactersXML));
        var stream = new FileStream(path, FileMode.Open);
        var container = serializer.Deserialize(stream) as CharactersXML;
        stream.Close();

        CharactersLoaded = new List<CharacterValues>();

        foreach (CharacterValues character in container.Characters)
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
    public List<CharacterValues> Characters = new List<CharacterValues>();
}