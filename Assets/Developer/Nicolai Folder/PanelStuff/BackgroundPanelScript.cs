using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BackgroundPanelScript : MonoBehaviour, IPointerClickHandler
{
    PanelClicked panelClickedScript;

    void Start()
    {
        panelClickedScript = GetComponentInChildren<PanelClicked>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
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
 