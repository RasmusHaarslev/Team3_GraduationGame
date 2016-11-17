using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class CampManager : MonoBehaviour
{
    public CampUpgrades Upgrades;
    public int GatherCost;
    public int GatherCostIncrease;

    public int BlacksmithCost;
    public int BlacksmithCostIncrease;

    public int LeaderHealthCost;
    public int LeaderHealthCostIncrease;

    public int LeaderStrengthCost;
    public int LeaderStrengthCostIncrease;

    public int MaxVillagesCost;
    public int MaxVillagesCostIncrease;

    public Text[] TextLevels;
    public Button[] Buttons;

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

    void Start()
    {
        LoadUpgrades();
    }

    private void SaveUpgrades()
    {
        var path = Path.Combine(PersistentData.GetPath(), "upgrades.xml");

        foreach (var but in Buttons)
            but.interactable = !Upgrades.UpgradeInProgress;

        var serializer = new XmlSerializer(typeof(CampUpgrades));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, Upgrades);
        stream.Close();
    }

    private void LoadUpgrades()
    {
        var path = Path.Combine(PersistentData.GetPath(), "upgrades.xml");

        if (File.Exists(path))
        {
            var serializer = new XmlSerializer(typeof(CampUpgrades));
            var stream = new FileStream(path, FileMode.Open);
            Upgrades = serializer.Deserialize(stream) as CampUpgrades;
            Upgrades.GetCurrency();
            SetLevels();
            stream.Close();
        }
        else {
            Upgrades = new CampUpgrades();
            SaveUpgrades();
        }
    }

    private void FinishUpgrade()
    {
        switch (Upgrades.UpgradeBought) {
            case "Gather":
                Upgrades.GatherLevel++;
                break;

            case "LeaderStrength":
                Upgrades.LeaderStrengthLevel++;
                break;

            case "LeaderHealth":
                Upgrades.LeaderHealthLevel++;
                break;

            case "Blacksmith":
                Upgrades.BlacksmithLevel++;
                break;

            case "Villages":
                Upgrades.MaxVillages++;
                break;

            default:
                break;
        }

        Upgrades.UpgradeInProgress = false;
        Upgrades.UpgradeBought = "";
        SaveUpgrades();
        SetLevels();
    }

    public double TimeLeftInSeconds()
    {
        if (Upgrades.UpgradeInProgress)
        {
            DateTime end = DateTime.Parse(PlayerPrefs.GetString("UpgradeEnd"));
            double timeLeft = (end - DateTime.Now).TotalSeconds;
            if (timeLeft < 0.0)
            {
                FinishUpgrade();
                return 0.0;
            }
            else
                return (int) timeLeft;
        }
        else
        {
            return 0.0;
        }
        
    }

    public void UpgradeGather()
    {
        if (Upgrades.UpgradeInProgress)
            return;

        var amountOfSeconds = GetTimeForUpgrade(Upgrades.GatherLevel);
        DateTime End = DateTime.Now.AddSeconds(amountOfSeconds);
        
        PlayerPrefs.SetString("UpgradeEnd",End.ToString());
        Upgrades.UpgradeInProgress = true;
        Upgrades.UpgradeBought = "Gather";
        Upgrades.Scrap -= GatherCost + (GatherCostIncrease * Upgrades.GatherLevel);
        SaveUpgrades();
    }

    public void UpgradeVillages()
    {
        if (Upgrades.UpgradeInProgress)
            return;

        var amountOfSeconds = GetTimeForUpgrade(Upgrades.MaxVillages);
        DateTime End = DateTime.Now.AddSeconds(amountOfSeconds);

        PlayerPrefs.SetString("UpgradeEnd", End.ToString());
        Upgrades.UpgradeInProgress = true;
        Upgrades.UpgradeBought = "Villages";
        Upgrades.Scrap -= MaxVillagesCost + (MaxVillagesCostIncrease * Upgrades.GatherLevel);
        SaveUpgrades();
    }

    public void UpgradeBlacksmith()
    {
        if (Upgrades.UpgradeInProgress)
            return;

        var amountOfSeconds = GetTimeForUpgrade(Upgrades.BlacksmithLevel);
        DateTime End = DateTime.Now.AddSeconds(amountOfSeconds);

        PlayerPrefs.SetString("UpgradeEnd", End.ToString());
        Upgrades.UpgradeInProgress = true;
        Upgrades.UpgradeBought = "Blacksmith";
        Upgrades.Scrap -= BlacksmithCost + (BlacksmithCostIncrease * Upgrades.GatherLevel);
        SaveUpgrades();
    }

    public void UpgradeLeaderHealth()
    {
        if (Upgrades.UpgradeInProgress)
            return;

        var amountOfSeconds = GetTimeForUpgrade(Upgrades.LeaderHealthLevel);
        DateTime End = DateTime.Now.AddSeconds(amountOfSeconds);

        PlayerPrefs.SetString("UpgradeEnd", End.ToString());
        Upgrades.UpgradeInProgress = true;
        Upgrades.UpgradeBought = "LeaderHealth";
        Upgrades.Scrap -= LeaderHealthCost + (LeaderHealthCostIncrease * Upgrades.GatherLevel);
        SaveUpgrades();
    }

    public void UpgradeLeaderStrength()
    {
        if (Upgrades.UpgradeInProgress)
            return;

        var amountOfSeconds = GetTimeForUpgrade(Upgrades.LeaderStrengthLevel);
        DateTime End = DateTime.Now.AddSeconds(amountOfSeconds);

        PlayerPrefs.SetString("UpgradeEnd", End.ToString());
        Upgrades.UpgradeInProgress = true;
        Upgrades.UpgradeBought = "LeaderStrength";
        Upgrades.Scrap -= LeaderStrengthCost + (LeaderStrengthCostIncrease * Upgrades.GatherLevel);
        SaveUpgrades();
    }

    private void SetLevels()
    {
        TextLevels[0].text = Upgrades.GatherLevel+"";
        TextLevels[1].text = Upgrades.BlacksmithLevel + "";
        TextLevels[2].text = Upgrades.MaxVillages + "";
        TextLevels[3].text = Upgrades.LeaderHealthLevel + "";
        TextLevels[4].text = Upgrades.LeaderStrengthLevel + "";

        foreach (var but in Buttons)
            but.interactable = true;
    }

    private int GetTimeForUpgrade(int level) {
        switch (level)
        {
            case 0:
                return 60;

            case 1:
                return 120;

            case 2:
                return 300;

            case 3:
                return 600;

            case 4:
                return 900;

            case 5:
                return 1200;

            case 6:
                return 1800;

            case 7:
                return 2700;

            case 8:
                return 3600;

            default:
                return 7200;
        }
    }
}

public class CampUpgrades
{
    #region State

    public bool UpgradeInProgress;
    public string UpgradeBought;

    #endregion

    #region Upgradeables

    public int GatherLevel;
    public int MaxVillages;
    public int BlacksmithLevel;
    public int LeaderHealthLevel;
    public int LeaderStrengthLevel;

    #endregion


    #region Currency

    public int Gold = 0;
    public int Scrap = 0;
    public int Food = 0;

    #endregion

    public CampUpgrades() { }

    public void GetCurrency()
    {
        Gold = PlayerPrefs.GetInt("Gold");
        Food = PlayerPrefs.GetInt("Food");
        Scrap = PlayerPrefs.GetInt("Scrap");
    }
}