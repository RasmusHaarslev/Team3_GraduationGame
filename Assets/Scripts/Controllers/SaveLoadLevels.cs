using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class SaveLoadLevels
{
    public static void SaveLevels(Dictionary<int, List<GameObject>> levelDictionary)
    {
        var levels = new LevelXML();

        foreach (KeyValuePair<int, List<GameObject>> entry in levelDictionary)
        {
            var row = new RowsXML();

            foreach (var node in entry.Value)
            {
                var currentNode = node.GetComponent<Node>();

                var xmlNode = new NodeXML();

                xmlNode.CampsInNode = currentNode.CampsInNode;

                xmlNode.NodeId = currentNode.NodeId;
                xmlNode.Level = currentNode.Level;
                xmlNode.TravelCost = currentNode.TravelCost;
                xmlNode.sceneSelection = currentNode.sceneSelection;
                xmlNode.CampsInNode = currentNode.CampsInNode;
                xmlNode.probabilityWolves = currentNode.probabilityWolves;
                xmlNode.probabilityTribes = currentNode.probabilityTribes;
                xmlNode.probabilityChoice = currentNode.probabilityChoice;
                xmlNode.wolveCamps = currentNode.wolveCamps;
                xmlNode.tribeCamps = currentNode.tribeCamps;
                xmlNode.choiceCamps = currentNode.choiceCamps;
                xmlNode.foodAmount = currentNode.foodAmount;
                xmlNode.coinAmount = currentNode.coinAmount;
                xmlNode.itemDropAmount = currentNode.itemDropAmount;
                xmlNode.isCleared = currentNode.isCleared;
                xmlNode.isScouted = currentNode.isScouted;
                xmlNode.canPlay = currentNode.canPlay;

                row.Nodes.Add(xmlNode);
            }
            levels.Rows.Add(row);
        }

        var path = Path.Combine(Application.persistentDataPath, "levels.xml");

        var serializer = new XmlSerializer(typeof(LevelXML));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, levels);
        stream.Close();
    }

    public static Dictionary<int, List<GameObject>> LoadLevels(Dictionary<int, List<GameObject>> levelDictionary)
    {
        var path = Path.Combine(Application.persistentDataPath, "levels.xml");

        var serializer = new XmlSerializer(typeof(LevelXML));
        var stream = new FileStream(path, FileMode.Open);
        var container = serializer.Deserialize(stream) as LevelXML;
        stream.Close();

        var loadedLevels = new Dictionary<int, List<GameObject>>();

        foreach (RowsXML row in container.Rows)
        {
            int currentRow = 0;
            List<GameObject> list = new List<GameObject>();

            foreach (var node in row.Nodes)
            {
                var nodeObject = new GameObject();
                var currentNode = nodeObject.AddComponent<Node>();

                var xmlNode = new NodeXML();

                xmlNode.CampsInNode = currentNode.CampsInNode;

                currentRow = currentNode.Level;

                currentNode.NodeId = xmlNode.NodeId;
                currentNode.Level = xmlNode.Level;
                currentNode.TravelCost = xmlNode.TravelCost;
                currentNode.sceneSelection = xmlNode.sceneSelection;
                currentNode.CampsInNode = xmlNode.CampsInNode;
                currentNode.probabilityWolves = xmlNode.probabilityWolves;
                currentNode.probabilityTribes = xmlNode.probabilityTribes;
                currentNode.probabilityChoice = xmlNode.probabilityChoice;
                currentNode.wolveCamps = xmlNode.wolveCamps;
                currentNode.tribeCamps = xmlNode.tribeCamps;
                currentNode.choiceCamps = xmlNode.choiceCamps;
                currentNode.foodAmount = xmlNode.foodAmount;
                currentNode.coinAmount = xmlNode.coinAmount;
                currentNode.itemDropAmount = xmlNode.itemDropAmount;
                currentNode.isCleared = xmlNode.isCleared;
                currentNode.isScouted = xmlNode.isScouted;
                currentNode.canPlay = xmlNode.canPlay;

                list.Add(nodeObject);
            }

            loadedLevels.Add(currentRow, list);
        }

        return loadedLevels;
    }
}

public class LevelXML
{
    [XmlArray("RowXMLs")]
    [XmlArrayItem("RowXML")]
    public List<RowsXML> Rows = new List<RowsXML>();
}

public class RowsXML
{
    [XmlArray("NodeXMLs")]
    [XmlArrayItem("NodeXML")]
    public List<NodeXML> Nodes = new List<NodeXML>();
}

public class NodeXML
{
    [XmlAttribute("nodeID")]
    public int NodeId;
    [XmlAttribute("level")]
    public int Level;
    [XmlAttribute("travelCost")]
    public int TravelCost;
    [XmlAttribute("sceneSelection")]
    public int sceneSelection;
    [XmlAttribute("CampsInNode")]
    public int CampsInNode;
    [XmlAttribute("probabilityWolves")]
    public int probabilityWolves;
    [XmlAttribute("probabilityTribes")]
    public int probabilityTribes;
    [XmlAttribute("probabilityChoice")]
    public int probabilityChoice;
    [XmlAttribute("wolveCamps")]
    public int wolveCamps;
    [XmlAttribute("tribeCamps")]
    public int tribeCamps;
    [XmlAttribute("choiceCamps")]
    public int choiceCamps;
    [XmlAttribute("foodAmount")]
    public int foodAmount;
    [XmlAttribute("coinAmount")]
    public int coinAmount;
    [XmlAttribute("itemDropAmount")]
    public int itemDropAmount;
    [XmlAttribute("isCleared")]
    public bool isCleared;
    [XmlAttribute("isScouted")]
    public bool isScouted;
    [XmlAttribute("canPlay")]
    public bool canPlay;
}