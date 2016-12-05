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
    public static Dictionary<int, GameObject> AllLevelsLoaded = new Dictionary<int, GameObject>();
    public static int maxRowsCleared = -1;
    public static GameObject lastNodeCleared;

    private static LevelsDatabase database;

    public static void SaveLevels(Dictionary<int, List<GameObject>> levelDictionary)
    {
        database = Resources.Load("ScriptableObjects/LevelsDatabase") as LevelsDatabase;
        database.Rows = new List<RowObject>();

        foreach (KeyValuePair<int, List<GameObject>> entry in levelDictionary)
        {
            var row = new RowObject();

            foreach (var node in entry.Value)
            {
                var currentNode = node.GetComponent<Node>();

                var xmlNode = new NodeObject();

                row.Level = currentNode.Level;

                xmlNode.CampsInNode = currentNode.CampsInNode;

                xmlNode.NodeId = currentNode.NodeId;
                xmlNode.TravelCost = currentNode.TravelCost;
                xmlNode.scoutCost = currentNode.scoutCost;
                xmlNode.sceneSelection = currentNode.sceneSelection;
                xmlNode.CampsInNode = currentNode.CampsInNode;
                xmlNode.probabilityWolves = currentNode.probabilityWolves;
                xmlNode.probabilityTribes = currentNode.probabilityTribes;
                xmlNode.probabilityChoice = currentNode.probabilityChoice;
                xmlNode.wolveCamps = currentNode.wolveCamps;
                xmlNode.tribeCamps = currentNode.tribeCamps;
                xmlNode.choiceCamps = currentNode.choiceCamps;
                xmlNode.foodAmount = currentNode.foodAmount;
                xmlNode.scrapAmount = currentNode.scrapAmount;
                xmlNode.goldTeethAmount = currentNode.goldTeethAmount;
                xmlNode.itemDropAmount = currentNode.itemDropAmount;
                xmlNode.isCleared = currentNode.isCleared;
                xmlNode.isScouted = currentNode.isScouted;
                xmlNode.isOpen = currentNode.isOpen;
                xmlNode.canPlay = currentNode.canPlay;

                foreach(var link in currentNode.Links)
                {
                    var li = new LinkObject();

                    li.FromID = link.From.GetComponent<Node>().NodeId;
                    li.ToID = link.To.GetComponent<Node>().NodeId;
                    li.Hierarchy = link.Hierarchy ? 1 : 0;

                    xmlNode.Links.Add(li);
                }

                row.Nodes.Add(xmlNode);
            }
            database.Rows.Add(row);
        }

        //var path = Path.Combine(PersistentData.GetPath(), "levels.xml");

        //var serializer = new XmlSerializer(typeof(LevelXML));
        //var stream = new FileStream(path, FileMode.Create);
        //serializer.Serialize(stream, levels);
        //stream.Close();
    }

    public static Dictionary<int, List<GameObject>> LoadLevels()
    {
        database = Resources.Load("ScriptableObjects/LevelsDatabase") as LevelsDatabase;

        if (database.Rows.Count() < 1)
        {
            Debug.LogError("No levels generated, reset game");
            return new Dictionary<int, List<GameObject>>();
        }

        AllLevelsLoaded = new Dictionary<int, GameObject>();

        var loadedLevels = new Dictionary<int, List<GameObject>>();

        foreach (RowObject row in database.Rows)
        {
            foreach (var node in row.Nodes)
            {
                var nodeObject = GameObject.Instantiate(Resources.Load("Prefabs/LevelSelection/CityNode", typeof(GameObject))) as GameObject;
                
                var currentNode = nodeObject.GetComponent<Node>();
                currentNode.CampsInNode = node.CampsInNode;

                currentNode.NodeId = node.NodeId;
                currentNode.Level = row.Level;
                currentNode.TravelCost = node.TravelCost;
                currentNode.scoutCost = node.scoutCost;
                currentNode.sceneSelection = node.sceneSelection;
                currentNode.CampsInNode = node.CampsInNode;
                currentNode.probabilityWolves = node.probabilityWolves;
                currentNode.probabilityTribes = node.probabilityTribes;
                currentNode.probabilityChoice = node.probabilityChoice;
                currentNode.wolveCamps = node.wolveCamps;
                currentNode.tribeCamps = node.tribeCamps;
                currentNode.choiceCamps = node.choiceCamps;
                currentNode.foodAmount = node.foodAmount;
                currentNode.scrapAmount = node.scrapAmount;
                currentNode.goldTeethAmount = node.goldTeethAmount;
                currentNode.itemDropAmount = node.itemDropAmount;
                currentNode.isCleared = node.isCleared;
                currentNode.isScouted = node.isScouted;
                currentNode.isOpen = node.isOpen;
                currentNode.canPlay = node.canPlay;

                currentNode.OnCreate(currentNode.NodeId);

                // Finds the max rows
                if (node.isCleared && row.Level > maxRowsCleared) { 
                    maxRowsCleared = row.Level;
                    lastNodeCleared = nodeObject;
                }

                AllLevelsLoaded.Add(node.NodeId, nodeObject);
            }
        }

        foreach (RowObject row in database.Rows)
        {
            int currentRow = row.Level;
            List<GameObject> list = new List<GameObject>();

            foreach (var node in row.Nodes)
            {
                var nodeObject = AllLevelsLoaded[node.NodeId];
                var currentNode = nodeObject.GetComponent<Node>();

                foreach (var link in node.Links)
                {
                    var li = new Link();

                    li.From = AllLevelsLoaded[link.FromID];
                    li.To = AllLevelsLoaded[link.ToID];
                    li.Hierarchy = link.Hierarchy == 1 ? true : false;

                    currentNode.Links.Add(li);
                }

                list.Add(nodeObject);
            }

            loadedLevels.Add(currentRow, list);
        }

        return loadedLevels;
    }
}