using UnityEngine;
using System.Collections;

public class LevelCanvasManager : MonoBehaviour
{

	public GameObject fleePopUp;

	void OnEnable()
	{
		EventManager.Instance.StartListening<FleeStateEvent>(ShowPopUp);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<FleeStateEvent>(ShowPopUp);
	}

    void OnApplicationQuit()
    {
        this.enabled = false;
    }

    public void ShowPopUp(FleeStateEvent e)
	{
		StartCoroutine(Flee());
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

	IEnumerator Flee()
	{
		yield return new WaitForSeconds(3);
		fleePopUp.SetActive(true);
		EventManager.Instance.TriggerEvent(new UIPanelActiveEvent());
		if (fleePopUp.transform.GetChild(0).GetComponent<ConfirmPanel>() != null)
		{
			fleePopUp.transform.GetChild(0).GetComponent<ConfirmPanel>().SetupText(null, "flee");
		}
		yield return null;
	}

	public void StopFlee()
	{
		EventManager.Instance.TriggerEvent(new UIPanelActiveEvent());
		EventManager.Instance.TriggerEvent(new StopFleeEvent());
	}

    public void DoneTutorial(int tutlevel)
    {
        if (tutlevel != 5) { 
            EventManager.Instance.TriggerEvent(new TutorialDone(tutlevel));
        }else
        {
            GameController.Instance.LoadScene("CampManagement");
        }
    }
}
