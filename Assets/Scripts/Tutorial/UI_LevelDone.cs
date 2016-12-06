using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_LevelDone : MonoBehaviour {

    public GameObject endingPanel;
    public Text txtResourcesFound;

    void OnEnable()
    {
        EventManager.Instance.StartListening<TutorialDone>(LevelDone);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<TutorialDone>(LevelDone);
    }

    public void LevelDone(TutorialDone e)
    {
		if (GameController.Instance.numberOfActiveUIs == 0)
		{
			EventManager.Instance.TriggerEvent(new UIPanelActiveEvent(false));
		}
		GameController.Instance.numberOfActiveUIs++;
		GameObject.FindGameObjectWithTag("Player").GetComponent<MoveScript>().enabled = false;
        endingPanel.SetActive(true);

        foreach (Transform child in endingPanel.transform.GetChild(0).GetChild(1).transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
