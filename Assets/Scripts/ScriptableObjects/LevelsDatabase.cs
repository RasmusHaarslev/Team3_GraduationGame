using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelsDatabase : ScriptableObject {
    public List<RowObject> Rows = new List<RowObject>();
}

[System.Serializable]
public class RowObject
{
    public List<NodeObject> Nodes = new List<NodeObject>();
    public int Level;
}

[System.Serializable]
public class NodeObject
{
    public int NodeId;
    public int TravelCost;
    public int scoutCost;
    public int sceneSelection;
    public int CampsInNode;
    public int probabilityWolves;
    public int probabilityTribes;
    public int probabilityChoice;
    public int wolveCamps;
    public int tribeCamps;
    public int choiceCamps;
    public int foodAmount;
    public int scrapAmount;
    public int goldTeethAmount;
    public int itemDropAmount;
    public bool isCleared;
    public bool isScouted;
    public bool isOpen;
    public bool canPlay;

    public List<LinkObject> Links = new List<LinkObject>();
}

[System.Serializable]
public class LinkObject
{
    public int FromID;
    public int ToID;
    public int Hierarchy;
}