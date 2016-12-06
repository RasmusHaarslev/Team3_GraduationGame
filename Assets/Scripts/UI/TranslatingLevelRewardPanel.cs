using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TranslatingLevelRewardPanel : MonoBehaviour {

    public Text txtLevelCleared;
    public Text txtResourcesFound;
    public Text txtItemsFound;
    
	void Start () {
        txtLevelCleared.text = TranslationManager.Instance.GetTranslation("LevelCleared");
        txtResourcesFound.text = TranslationManager.Instance.GetTranslation("ResourcesFound");
        txtItemsFound.text = TranslationManager.Instance.GetTranslation("ItemsFound");
    }	
}
