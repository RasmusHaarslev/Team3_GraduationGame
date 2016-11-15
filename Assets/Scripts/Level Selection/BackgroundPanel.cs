using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BackgroundPanel : MonoBehaviour, IPointerClickHandler
{
    public GameObject PopUpPanel;
    PanelClicked panelClickedScript;

    void Start()
    {
        panelClickedScript = PopUpPanel.GetComponentInChildren<PanelClicked>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (panelClickedScript.isClicked)
        {
            // DEACTIVATE POP UP
            foreach (Transform child in PopUpPanel.transform)
            {
                if (child.gameObject.activeSelf == true)
                {
                    child.gameObject.SetActive(false);

                }
            }
            PopUpPanel.SetActive(false);
            gameObject.SetActive(false);
        }

    }

}
