using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CampTopPanel : MonoBehaviour {

    public Text VillageCount;
    public Text FoodCount;
    public Text ScrapCount;
	public Text PremiumCount;

    public Text txtDaysSurvived;

    void OnEnable()
    {
        EventManager.Instance.StartListening<ResourcesUpdated>(UpdateResources);
        EventManager.Instance.StartListening<LanguageChanged>(UpdateText);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<ResourcesUpdated>(UpdateResources);
        EventManager.Instance.StopListening<LanguageChanged>(UpdateText);
    }

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

    // Use this for initialization
    void Start () {
        VillageCount.text = GameController.Instance._VILLAGERS.ToString();
        FoodCount.text = GameController.Instance._FOOD.ToString();
        ScrapCount.text = GameController.Instance._SCRAPS.ToString();
        PremiumCount.text = GameController.Instance._PREMIUM.ToString();
        txtDaysSurvived.text = GameController.Instance._DAYS_SURVIVED.ToString() + " " + TranslationManager.Instance.GetTranslation("DaysSurvived");
    }
	
	// Update is called once per frame
	public void UpdateResources(ResourcesUpdated e) {
        VillageCount.text = GameController.Instance._VILLAGERS.ToString();
        FoodCount.text = GameController.Instance._FOOD.ToString();
        ScrapCount.text = GameController.Instance._SCRAPS.ToString();
        PremiumCount.text = GameController.Instance._PREMIUM.ToString();
    }

    public void UpdateText(LanguageChanged e) {
        txtDaysSurvived.text = GameController.Instance._DAYS_SURVIVED.ToString() + " " + TranslationManager.Instance.GetTranslation("DaysSurvived");
    }
}
