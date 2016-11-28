using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class CampUIController : MonoBehaviour
{
    public Text[] TextLevels;
	public Text[] TextCosts;
    public Button[] Buttons;
    public Button FinishNow;
	public GameObject FinishConfirmationPanel;
	public GameObject InsufficientPremiumPanel;
	public GameObject UpgradeConfirmationPanel;
	public GameObject InsufficientScrapsPanel;
	public GameObject ConfirmationShade;

    private CampManager campManager;

    void Start()
    {
        campManager = GetComponent<CampManager>();
    }

    void OnEnable()
    {
        campManager = GetComponent<CampManager>();
        campManager.LoadUpgrades();
        SetLevels();
        SetCosts();
        FinishNow.interactable = campManager.Upgrades.UpgradeInProgress;
    }

    public void PerformUpgrade()
    {
        DateTime End = DateTime.Now.AddSeconds(campManager.amountOfSeconds);

        PlayerPrefs.SetString("UpgradeEnd", End.ToString());
        campManager.Upgrades.UpgradeInProgress = true;
        campManager.Upgrades.UpgradeBought = campManager.tempUpgradeName;
        campManager.Upgrades.Scrap -= campManager.tempCost;

		EventManager.Instance.TriggerEvent(new ChangeResources(scraps: -campManager.tempCost));
        FinishNow.interactable = true;
        campManager.SaveUpgrades();
    }

    public double TimeLeftInSeconds()
    {
        if (campManager.Upgrades.UpgradeInProgress)
        {
            DateTime end = DateTime.Parse(PlayerPrefs.GetString("UpgradeEnd"));
            double timeLeft = (end - DateTime.Now).TotalSeconds;
            if (timeLeft < 0.0)
            {
                campManager.FinishUpgrade();
                SetLevels();
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
        campManager.Upgrades.UpgradeBought = "";
        campManager.Upgrades.UpgradeInProgress = false;
        campManager.tempCost = 0;
    }

    public void FinishUpgradeClicked()
    {
		if (GameController.Instance._PREMIUM < campManager.FinishUpgradeCost)
			InsufficientPremiumPanel.SetActive (true);
		else
			FinishConfirmationPanel.SetActive (true);

		ConfirmationShade.SetActive (true);
    }

    public void FinishUpgradeNow()
    {
        campManager.FinishUpgradeNow();

        // Update premium resource in GameController.
        EventManager.Instance.TriggerEvent(new ChangeResources(premium: -campManager.FinishUpgradeCost));

        campManager.FinishUpgrade();
        SetLevels();
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
        campManager.UpgradeGather();
		OpenUpgradePopUp (campManager.tempCost);
    }

    public void UpgradeVillages()
    {
        campManager.UpgradeVillages();
		OpenUpgradePopUp (campManager.tempCost);
    }

    public void UpgradeLeaderHealth()
    {
        campManager.UpgradeLeaderHealth();
		OpenUpgradePopUp (campManager.tempCost);
    }

    public void UpgradeLeaderStrength()
    {
        campManager.UpgradeLeaderStrength();
        OpenUpgradePopUp (campManager.tempCost);
    }

    private void SetLevels()
    {
        TextLevels[0].text = campManager.Upgrades.GatherLevel+"";
        TextLevels[1].text = campManager.Upgrades.MaxVillages + "";
        TextLevels[2].text = campManager.Upgrades.LeaderHealthLevel + "";
        TextLevels[3].text = campManager.Upgrades.LeaderStrengthLevel + "";
    }

	private void SetCosts()
	{
		TextCosts[0].text = campManager.GatherCost + campManager.Upgrades.GatherLevel * campManager.GatherCostIncrease + "";
		TextCosts[1].text = campManager.MaxVillagesCost + campManager.Upgrades.MaxVillages * campManager.MaxVillagesCostIncrease + "";
		TextCosts[2].text = campManager.LeaderHealthCost + campManager.Upgrades.LeaderHealthLevel * campManager.LeaderHealthCostIncrease + "";
		TextCosts[3].text = campManager.LeaderStrengthCost + campManager.Upgrades.LeaderStrengthLevel * campManager.LeaderStrengthCostIncrease + "";
	}
    #endregion
}