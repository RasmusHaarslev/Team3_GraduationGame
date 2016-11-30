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
			EventManager.Instance.TriggerEvent(new UIPanelActiveEvent());
			foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf == true)
                {
                    child.gameObject.SetActive(false);
                }
            }
            Debug.Log("Clicking : " + gameObject);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

    }

}
