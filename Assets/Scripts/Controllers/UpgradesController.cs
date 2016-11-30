using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UpgradesController : MonoBehaviour {

    public GameObject DailyPanel;
    public Text Text;
    public Text Amount;

    public Text Info;

    // Use this for initialization
    void Awake() {
        var currentDate = DateTime.Today;

        if (PlayerPrefs.HasKey("GatherDate"))
        {
            Amount.text = "+ " + (CampManager.Instance.Upgrades.GatherLevel * 2) + " Food";

            var lastGather = DateTime.Parse(PlayerPrefs.GetString("GatherDate"));
            PlayerPrefs.SetString("GatherDate", (currentDate.ToString()));

            if (lastGather.Date < currentDate.Date){
                Info.gameObject.SetActive(false);
                Text.gameObject.SetActive(true);
                Amount.gameObject.SetActive(true);
                DailyPanel.SetActive(true);

                var currentFood = PlayerPrefs.GetInt("Food");
                PlayerPrefs.SetInt("Food", currentFood + (CampManager.Instance.Upgrades.GatherLevel * 2));
            }
            else
            {
                DailyPanel.SetActive(false);
            }
        }
        else
        {
            Info.gameObject.SetActive(true);
            Text.gameObject.SetActive(false);
            Amount.gameObject.SetActive(false);
            DailyPanel.SetActive(true);

            PlayerPrefs.SetString("GatherDate", (currentDate.AddDays(-1).ToString()));
        }
    }
}
