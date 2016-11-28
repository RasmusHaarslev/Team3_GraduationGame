using UnityEngine;
using System.Collections;

public class LevelCanvasManager : MonoBehaviour {

    public GameObject fleePopUp;

    void OnEnable()
    {
        EventManager.Instance.StartListening<FleeStateEvent>(ShowPopUp);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<FleeStateEvent>(ShowPopUp);
    }

    public void ShowPopUp(FleeStateEvent e)
    {
        fleePopUp.SetActive(true);
        fleePopUp.transform.GetChild(0).GetComponent<ConfirmPanel>().SetupText(null, "flee");
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

    public void LoadFleeCutScene()
    {
        GameController.Instance.LoadScene("LevelFleeCutscene");
    }
}
