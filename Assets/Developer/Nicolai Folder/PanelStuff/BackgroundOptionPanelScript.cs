using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BackgroundOptionPanelScript : MonoBehaviour, IPointerClickHandler
{
    PanelClicked panelClickedScript;

    void Start()
    {
        panelClickedScript = GetComponentInChildren<PanelClicked>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        if (panelClickedScript.isClicked)
        {
			if (GameController.Instance.numberOfActiveUIs == 1)
			{
				EventManager.Instance.TriggerEvent(new UIPanelActiveEvent(true));
			}
			GameController.Instance.numberOfActiveUIs--;
			foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf == true)
                {
                    child.gameObject.SetActive(false);
                }
            }
            //Debug.Log("Clicking : " + gameObject);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        } 

    }

}
