using UnityEngine;
using System.Collections;
using System;

public class UpgradesController : MonoBehaviour {

	// Use this for initialization
	void Awake() {
        var currentDate = DateTime.Today;

        if (PlayerPrefs.HasKey("GatherDate"))
        {
            var lastGather = DateTime.Parse(PlayerPrefs.GetString("GatherDate"));

            if(lastGather.Date < currentDate.Date){
                var currentFood = PlayerPrefs.GetInt("Food");
                PlayerPrefs.SetInt("Food", currentFood + (CampManager.Instance.Upgrades.GatherLevel * 2));
            }
        }

        PlayerPrefs.SetString("GatherDate", (currentDate.ToString()));
    }
}
