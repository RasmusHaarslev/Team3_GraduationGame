using UnityEngine;
using System.Collections;

public class LevelCanvasManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void DisplayEndLootItems(EquippableitemValues[] newItemsValues)
    {
        print("executing loot items!");
        //display them on the panel
        EquippableItemUIListController listController = GetComponentInChildren<EquippableItemUIListController>(true);
        listController.GenerateItemsList(newItemsValues);
        //activate new items panel
        listController.transform.parent.parent.gameObject.SetActive(true);
    }

}
