using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetupText : MonoBehaviour {

    public Text LevelDifficulty;
    public Text WolveCamps;
    public Text TribeCamps;
    public Text ChoiceCamps;
    public Text FoodAmount;
    public Text ScrapAmount;
    public Text ItemDropAmount;

    // Use this for initialization
    void Start () {
        LevelDifficulty.text = "Level Difficulty : " + PlayerPrefs.GetInt("LevelDifficulty");
        WolveCamps.text = "Wolve Camps : " + PlayerPrefs.GetInt("WolveCamps");
        TribeCamps.text = "Tribe Camps : " + PlayerPrefs.GetInt("TribeCamps");
        ChoiceCamps.text = "Choice Camps : " + PlayerPrefs.GetInt("ChoiceCamps");
        FoodAmount.text = "Food Amount : " + PlayerPrefs.GetInt("FoodAmount");
        ScrapAmount.text = "Scrap Amount : " + PlayerPrefs.GetInt("ScrapAmount");
        ItemDropAmount.text = "Item Drop Amount : " + PlayerPrefs.GetInt("ItemDropAmount");
    }

    public void GoToCamp()
    {
        GameController.Instance.LoadScene("CampManagement");
    }
}
