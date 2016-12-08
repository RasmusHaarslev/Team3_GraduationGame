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

    public int LeaderHealthCost;
    public int LeaderHealthCostIncrease;

    public int LeaderStrengthCost;
    public int LeaderStrengthCostIncrease;

    public int MaxVillagesCost;
    public int MaxVillagesCostIncrease;

    [Tooltip("The amount of gold it costs to finish an upgrade.")]
    public int FinishUpgradeCost;

    #region
    public int Level1_Time;
    public int Level2_Time;
    public int Level3_Time;
    public int Level4_Time;
    public int Level5_Time;
    public int Level6_Time;
    public int Level7_Time;
    public int Level8_Time;
    public int Level9_Above_Time;
    #endregion

    #region
    public double amountOfSeconds = 0.0;
    public string tempUpgradeName = "";
    public int tempCost = 0;
    public int tempGold = 0;
    #endregion

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

    void Awake()
    {

    }

    public void SaveUpgrades()
    {
        var path = Path.Combine(PersistentData.GetPath(), "upgrades.xml");

        var serializer = new XmlSerializer(typeof(CampUpgrades));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, Upgrades);
        stream.Close();
    }

    public void LoadUpgrades()
    {
        var path = Path.Combine(PersistentData.GetPath(), "upgrades.xml");

        if (File.Exists(path))
        {
            var serializer = new XmlSerializer(typeof(CampUpgrades));
            var stream = new FileStream(path, FileMode.Open);
            Upgrades = serializer.Deserialize(stream) as CampUpgrades;
            stream.Close();

            Upgrades.GetCurrency();
        }
        else
        {
            Upgrades = new CampUpgrades();
            SaveUpgrades();
        }
    }

    public void PerformUpgrade()
    {
        DateTime End = DateTime.Now.AddSeconds(amountOfSeconds);

        PlayerPrefs.SetString("UpgradeEnd", End.ToString());
        Upgrades.UpgradeInProgress = true;
        Upgrades.UpgradeBought = tempUpgradeName;
        Upgrades.Scrap -= tempCost;

		EventManager.Instance.TriggerEvent(new ChangeResources(scraps: -tempCost));

        SaveUpgrades();
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

    public void CancelUpgrade()
    {
        Upgrades.UpgradeBought = "";
        Upgrades.UpgradeInProgress = false;
        tempCost = 0;
    }

    public void FinishUpgradeNow()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_buyGold, gameObject);
        Upgrades.UpgradeInProgress = false;
        Upgrades.UpgradeBought = tempUpgradeName;
        Upgrades.Premium -= FinishUpgradeCost;

        // Update premium resource in GameController.
        EventManager.Instance.TriggerEvent(new ChangeResources(premium: -FinishUpgradeCost));

        FinishUpgrade();
    }

    public void FinishUpgrade()
    {
        switch (Upgrades.UpgradeBought)
        {
            case "Gather":
                Upgrades.GatherLevel++;
                break;

            case "LeaderStrength":
                Upgrades.LeaderStrengthLevel++;
                break;

            case "LeaderHealth":
                Upgrades.LeaderHealthLevel++;
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
    }

    #region Upgrade Buttons

    public void UpgradeGather()
    {
        tempUpgradeName     = "Gather";
        tempCost            = GatherCost + (GatherCostIncrease * Upgrades.GatherLevel);
        amountOfSeconds     = GetTimeForUpgrade(Upgrades.GatherLevel);
    }

    public void UpgradeVillages()
    {
        tempUpgradeName = "Villages";
        tempCost = MaxVillagesCost + (MaxVillagesCostIncrease * Upgrades.MaxVillages);
        amountOfSeconds = GetTimeForUpgrade(Upgrades.MaxVillages);
    }

    public void UpgradeLeaderHealth()
    {
        tempUpgradeName = "LeaderHealth";
        tempCost = LeaderHealthCost + (LeaderHealthCostIncrease * Upgrades.LeaderHealthLevel);
        amountOfSeconds = GetTimeForUpgrade(Upgrades.LeaderHealthLevel);
    }

    public void UpgradeLeaderStrength()
    {
        tempUpgradeName = "LeaderStrength";
        tempCost = LeaderStrengthCost + (LeaderStrengthCostIncrease * Upgrades.LeaderStrengthLevel);
        amountOfSeconds = GetTimeForUpgrade(Upgrades.LeaderStrengthLevel);
    }

    #endregion

    private int GetTimeForUpgrade(int level) {
        switch (level)
        {
            case 2:
                return Level1_Time;

            case 3:
                return Level2_Time;

            case 4:
                return Level3_Time;

            case 5:
                return Level4_Time;

            case 6:
                return Level5_Time;

            case 7:
                return Level6_Time;

            case 8:
                return Level7_Time;

            case 9:
                return Level8_Time;

            case 10:
                return Level9_Above_Time;

            default:
                return Level9_Above_Time;
        }
    }
}

[Serializable]
public class CampUpgrades
{
    #region State

    public bool UpgradeInProgress = false;
    public string UpgradeBought = "";

    #endregion

    #region Upgradeables

    public int GatherLevel = 1;
    public int MaxVillages = 1;
    public int BlacksmithLevel = 1;
    public int LeaderHealthLevel = 1;
    public int LeaderStrengthLevel = 1;

    #endregion

    #region Currency

    public int Premium = 0;
    public int Scrap = 0;
    public int Food = 0;
    public int Villagers = 0;

    #endregion

    public CampUpgrades() { }

    public void GetCurrency()
    {
        Premium = PlayerPrefs.GetInt(StringResources.Premium);
        Food = PlayerPrefs.GetInt(StringResources.Food);
        Scrap = PlayerPrefs.GetInt(StringResources.Scrap);
        Villagers = PlayerPrefs.GetInt(StringResources.Villagers);
    }
}