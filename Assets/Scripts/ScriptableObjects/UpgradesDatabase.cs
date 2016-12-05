using UnityEngine;
using System.Collections;

public class UpgradesDatabase : ScriptableObject {
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
    public int Gold = 0;
    public int Scrap = 0;
    public int Food = 0;
    public int Villages = 0;
    #endregion

    public void GetCurrency()
    {
        Gold = PlayerPrefs.GetInt("Premium");
        Food = PlayerPrefs.GetInt("Food");
        Scrap = PlayerPrefs.GetInt("Scraps");
        Villages = PlayerPrefs.GetInt("Villages");
    }

    public void SetCurrency()
    {
        PlayerPrefs.SetInt("Premium", Gold);
        PlayerPrefs.SetInt("Food", Food);
        PlayerPrefs.SetInt("Scraps", Scrap);
        PlayerPrefs.SetInt("Villages", Villages);
    }
}