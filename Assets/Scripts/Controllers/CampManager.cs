using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CampManager : MonoBehaviour
{
    public CampUpgrades Upgrades;

    #region Setup Instance
    private static CampManager _instance;

    public static CampManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("CampManager");
                var manager = go.AddComponent<CampManager>();
                manager.LoadUpgrades();
                _instance = manager;
            }
            return _instance;
        }
    }
    #endregion

    private void SaveUpgrades()
    {
        var path = Path.Combine(Application.persistentDataPath, "upgrades.xml");

        var serializer = new XmlSerializer(typeof(LevelXML));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, Upgrades);
        stream.Close();
    }

    private void LoadUpgrades()
    {
        var path = Path.Combine(Application.persistentDataPath, "upgrades.xml");

        if (File.Exists(path))
        {
            var serializer = new XmlSerializer(typeof(LevelXML));
            var stream = new FileStream(path, FileMode.Open);
            Upgrades = serializer.Deserialize(stream) as CampUpgrades;
            stream.Close();
        }
        else {
            Upgrades = new CampUpgrades();
            SaveUpgrades();
        }

    }
}

public class CampUpgrades
{
    #region Upgradeables

    [XmlAttribute]
    public int GatherLevel = 0;



    #endregion
}