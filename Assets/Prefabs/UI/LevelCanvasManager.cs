using UnityEngine;
using System.Collections;

public class LevelCanvasManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void DisplayEndLootItems(EquippableitemValues[] newItemsValues)
    {
        //display them on the panel
        EquippableItemUIListController listController = GetComponentInChildren<EquippableItemUIListController>();
        listController.GenerateItemsList(newItemsValues);
        //activate new items panel
        listController.transform.parent.gameObject.SetActive(true);
    }

}
