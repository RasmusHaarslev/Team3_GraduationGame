using UnityEngine;
using System.Collections;

public class GoToCamp : MonoBehaviour {

    public void LoadCampManagement()
    {
        EventManager.Instance.TriggerEvent(new ChangeResources(food: PlayerPrefs.GetInt(StringResources.FoodAmountPrefsName)));
        GameController.Instance.LoadScene("CampManagement");
    }
}
