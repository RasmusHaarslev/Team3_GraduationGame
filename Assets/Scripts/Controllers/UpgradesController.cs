using UnityEngine;
using System.Collections;
using System;

public class UpgradesController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var currentDate = DateTime.Today;

        if (PlayerPrefs.HasKey(""))
        {
            var lastGather = DateTime.Parse(PlayerPrefs.GetString("GatherDate"));

            if(lastGather.Date < currentDate.Date){
                var currentFood = PlayerPrefs.GetInt("Food");
                PlayerPrefs.SetInt("Food", currentFood + (CampManager.Instance.Upgrades.GatherLevel * 3));
            }
        }

        PlayerPrefs.SetString("GatherDate", currentDate.ToString());
    }

    
}
