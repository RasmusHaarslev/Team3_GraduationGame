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

    void Start()
    {

    }

    void OnEnable()
    {
        SetLevels();
        SetCosts();
        FinishNow.interactable = CampManager.Instance.Upgrades.UpgradeInProgress;
    }

    public void PerformUpgrade()
    {
        DateTime End = DateTime.Now.AddSeconds(CampManager.Instance.amountOfSeconds);

        PlayerPrefs.SetString("UpgradeEnd", End.ToString());
        CampManager.Instance.Upgrades.UpgradeInProgress = true;
        CampManager.Instance.Upgrades.UpgradeBought = CampManager.Instance.tempUpgradeName;
        CampManager.Instance.Upgrades.Scrap -= CampManager.Instance.tempCost;

		EventManager.Instance.TriggerEvent(new ChangeResources(scraps: -CampManager.Instance.tempCost));

        CampManager.Instance.SaveUpgrades();
    }

    public double TimeLeftInSeconds()
    {
        if (CampManager.Instance.Upgrades.UpgradeInProgress)
        {
            DateTime end = DateTime.Parse(PlayerPrefs.GetString("UpgradeEnd"));
            double timeLeft = (end - DateTime.Now).TotalSeconds;
            if (timeLeft < 0.0)
            {
                CampManager.Instance.FinishUpgrade();
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
        CampManager.Instance.Upgrades.UpgradeBought = "";
        CampManager.Instance.Upgrades.UpgradeInProgress = false;
        CampManager.Instance.tempCost = 0;
    }

    public void FinishUpgradeClicked()
    {
		if (GameController.Instance._PREMIUM < CampManager.Instance.FinishUpgradeCost)
			InsufficientPremiumPanel.SetActive (true);
		else
			FinishConfirmationPanel.SetActive (true);

		ConfirmationShade.SetActive (true);
    }

    public void FinishUpgradeNow()
    {
        CampManager.Instance.FinishUpgradeNow();

        // Update premium resource in GameController.
        EventManager.Instance.TriggerEvent(new ChangeResources(premium: -CampManager.Instance.FinishUpgradeCost));

        CampManager.Instance.FinishUpgrade();
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
        CampManager.Instance.UpgradeGather();
		OpenUpgradePopUp (CampManager.Instance.tempCost);
    }

    public void UpgradeVillages()
    {
        CampManager.Instance.UpgradeVillages();
		OpenUpgradePopUp (CampManager.Instance.tempCost);
    }

    public void UpgradeLeaderHealth()
    {
        CampManager.Instance.UpgradeLeaderHealth();
		OpenUpgradePopUp (CampManager.Instance.tempCost);
    }

    public void UpgradeLeaderStrength()
    {
        CampManager.Instance.UpgradeLeaderStrength();
        OpenUpgradePopUp (CampManager.Instance.tempCost);
    }

    private void SetLevels()
    {
        TextLevels[0].text = CampManager.Instance.Upgrades.GatherLevel+"";
        TextLevels[1].text = CampManager.Instance.Upgrades.MaxVillages + "";
        TextLevels[2].text = CampManager.Instance.Upgrades.LeaderHealthLevel + "";
        TextLevels[3].text = CampManager.Instance.Upgrades.LeaderStrengthLevel + "";
    }

	private void SetCosts()
	{
		TextCosts[0].text = CampManager.Instance.GatherCost + CampManager.Instance.Upgrades.GatherLevel * CampManager.Instance.GatherCostIncrease + "";
		TextCosts[1].text = CampManager.Instance.MaxVillagesCost + CampManager.Instance.Upgrades.MaxVillages * CampManager.Instance.MaxVillagesCostIncrease + "";
		TextCosts[2].text = CampManager.Instance.LeaderHealthCost + CampManager.Instance.Upgrades.LeaderHealthLevel * CampManager.Instance.LeaderHealthCostIncrease + "";
		TextCosts[3].text = CampManager.Instance.LeaderStrengthCost + CampManager.Instance.Upgrades.LeaderStrengthLevel * CampManager.Instance.LeaderStrengthCostIncrease + "";
	}
    #endregion
}