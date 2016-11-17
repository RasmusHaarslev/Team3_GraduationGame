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
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf == true)
                {
                    child.gameObject.SetActive(false);

                }
            }
            gameObject.SetActive(false);
        }

    }

}
