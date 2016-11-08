using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CampTopPanel : MonoBehaviour {

    public Text VillageCount;
    public Text FoodCount;
    public Text CoinsCount;

    // Use this for initialization
    void Start () {
        Debug.Log(GameController.Instance);
        VillageCount.text = GameController.Instance._VILLAGERS.ToString();
        FoodCount.text = GameController.Instance._FOOD.ToString();
        CoinsCount.text = GameController.Instance._COINS.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
