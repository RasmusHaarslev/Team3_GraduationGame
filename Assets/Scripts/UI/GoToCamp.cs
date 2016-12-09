using UnityEngine;
using System.Collections;

public class GoToCamp : MonoBehaviour {

    public void LoadCampManagement()
    {
		PlayerPrefs.SetInt("GameplayTutorialCompleted", 1);
		EventManager.Instance.TriggerEvent(new ChangeResources(food: PlayerPrefs.GetInt(StringResources.FoodAmountPrefsName)));
		EventManager.Instance.TriggerEvent(new EndSceneTransitionEvent("CampManagement"));
		//GameController.Instance.LoadScene("CampManagement");
    }
}
