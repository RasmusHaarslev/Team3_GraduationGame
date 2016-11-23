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

    public Text[] TextLevels;
	public Text[] TextCosts;
    public Button[] Buttons;
    public Button FinishNow;
	public GameObject FinishConfirmationPanel;
	public GameObject InsufficientPremiumPanel;
	public GameObject UpgradeConfirmationPanel;
	public GameObject InsufficientScrapsPanel;
	public GameObject ConfirmationShade;

    #region
    private double amountOfSeconds = 0.0;
    private string tempUpgradeName = "";
    private int tempCost = 0;
    private int tempGold = 0;
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

    void Start()
    {
        LoadUpgrades();
    }

    private void SaveUpgrades()
    {
        var path = Path.Combine(PersistentData.GetPath(), "upgrades.xml");

        foreach (var but in Buttons)
            but.interactable = !Upgrades.UpgradeInProgress;

        FinishNow.interactable = Upgrades.UpgradeInProgress;

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
			FinishNow.interactable = Upgrades.UpgradeInProgress;

            SetLevels();
			SetCosts ();
            stream.Close();
        }
        else {
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

    public void FinishUpgradeClicked()
    {
		if (GameController.Instance._PREMIUM < FinishUpgradeCost)
			InsufficientPremiumPanel.SetActive (true);
		else
			FinishConfirmationPanel.SetActive (true);

		ConfirmationShade.SetActive (true);
    }

    public void FinishUpgradeNow()
    {
        Upgrades.UpgradeInProgress = true;
        Upgrades.UpgradeBought = tempUpgradeName;
        Upgrades.Gold -= FinishUpgradeCost;

        // Update premium resource in GameController.
        EventManager.Instance.TriggerEvent(new ChangeResources(premium: -FinishUpgradeCost));

        FinishUpgrade();
    }

    private void FinishUpgrade()
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
        SetLevels();
		SetCosts ();
    }

    #region Upgrade Buttons

	private void OpenUpgradePopUp(int cost) {
		if (cost > GameController.Instance._SCRAPS)
			InsufficientScrapsPanel.SetActive (true);
		else
			UpgradeConfirmationPanel.SetActive (true);

		ConfirmationShade.SetActive (true);
	}

    public void UpgradeGather()
    {
        tempUpgradeName     = "Gather";
        tempCost            = GatherCost + (GatherCostIncrease * Upgrades.GatherLevel);
        amountOfSeconds     = GetTimeForUpgrade(Upgrades.GatherLevel);

		OpenUpgradePopUp (tempCost);
    }

    public void UpgradeVillages()
    {
        tempUpgradeName = "Villages";
        tempCost = MaxVillagesCost + (MaxVillagesCostIncrease * Upgrades.MaxVillages);
        amountOfSeconds = GetTimeForUpgrade(Upgrades.MaxVillages);

		OpenUpgradePopUp (tempCost);
    }

    public void UpgradeLeaderHealth()
    {
        tempUpgradeName = "LeaderHealth";
        tempCost = LeaderHealthCost + (LeaderHealthCostIncrease * Upgrades.LeaderHealthLevel);
        amountOfSeconds = GetTimeForUpgrade(Upgrades.LeaderHealthLevel);

		OpenUpgradePopUp (tempCost);
    }

    public void UpgradeLeaderStrength()
    {
        tempUpgradeName = "LeaderStrength";
        tempCost = LeaderStrengthCost + (LeaderStrengthCostIncrease * Upgrades.LeaderStrengthLevel);
        amountOfSeconds = GetTimeForUpgrade(Upgrades.LeaderStrengthLevel);

		OpenUpgradePopUp (tempCost);
    }

    private void SetLevels()
    {
        TextLevels[0].text = Upgrades.GatherLevel+"";
        TextLevels[1].text = Upgrades.MaxVillages + "";
        TextLevels[2].text = Upgrades.LeaderHealthLevel + "";
        TextLevels[3].text = Upgrades.LeaderStrengthLevel + "";
    }

	private void SetCosts()
	{
		TextCosts[0].text = GatherCost + Upgrades.GatherLevel * GatherCostIncrease + "";
		TextCosts[1].text = MaxVillagesCost + Upgrades.MaxVillages * MaxVillagesCostIncrease + "";
		TextCosts[2].text = LeaderHealthCost + Upgrades.LeaderHealthLevel * LeaderHealthCostIncrease + "";
		TextCosts[3].text = LeaderStrengthCost + Upgrades.LeaderStrengthLevel * LeaderStrengthCostIncrease + "";
	}

    #endregion

    private int GetTimeForUpgrade(int level) {
        switch (level)
        {
            case 0:
                return Level1_Time;

            case 1:
                return Level2_Time;

            case 2:
                return Level3_Time;

            case 3:
                return Level4_Time;

            case 4:
                return Level5_Time;

            case 5:
                return Level6_Time;

            case 6:
                return Level7_Time;

            case 7:
                return Level8_Time;

            case 8:
                return Level9_Above_Time;

            default:
                return Level9_Above_Time;
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
        Gold = PlayerPrefs.GetInt("Premium");
        Food = PlayerPrefs.GetInt("Food");
        Scrap = PlayerPrefs.GetInt("Scrap");
    }
}