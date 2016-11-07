using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetupText : MonoBehaviour {

    public Text LevelDifficulty;
    public Text WolveCamps;
    public Text TribeCamps;
    public Text ChoiceCamps;
    public Text FoodAmount;
    public Text CoinAmount;
    public Text ItemDropAmount;

    // Use this for initialization
    void Start () {
        LevelDifficulty.text = "Level Difficulty : " + PlayerPrefs.GetInt("LevelDifficulty").ToString();
        WolveCamps.text = "Wolve Camps : " + PlayerPrefs.GetInt("WolveCamps").ToString();
        TribeCamps.text = "Tribe Camps : " + PlayerPrefs.GetInt("TribeCamps").ToString();
        ChoiceCamps.text = "Choice Camps : " + PlayerPrefs.GetInt("ChoiceCamps").ToString();
        FoodAmount.text = "Food Amount : " + PlayerPrefs.GetInt("FoodAmount").ToString();
        CoinAmount.text = "Coin Amount : " + PlayerPrefs.GetInt("CoinAmount").ToString();
        ItemDropAmount.text = "Item Drop Amount : " + PlayerPrefs.GetInt("ItemDropAmount").ToString();
    }

    public void GoToCamp()
    {
        GameController.Instance.LoadScene(0);
    }
}
