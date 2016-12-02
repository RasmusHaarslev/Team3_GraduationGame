using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_LevelDone : MonoBehaviour {

    public GameObject endingPanel;

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
        GameObject.FindGameObjectWithTag("Player").GetComponent<MoveScript>().enabled = false;
        endingPanel.SetActive(true);

        foreach (Transform child in endingPanel.transform.GetChild(0).GetChild(1).transform)
        {
            Debug.Log(child.name);
            child.gameObject.SetActive(true);
        }
    }
}
