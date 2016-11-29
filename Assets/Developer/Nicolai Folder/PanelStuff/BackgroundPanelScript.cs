using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BackgroundPanelScript : MonoBehaviour, IPointerClickHandler
{
    PanelClicked panelClickedScript;
    PanelScript panelScript;
    void Start()
    {
        panelClickedScript = GetComponentInChildren<PanelClicked>();
        panelScript = GameObject.FindGameObjectWithTag("CampPanel").GetComponent<PanelScript>();
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
                    if(gameObject.name != "LevelSelection" && gameObject.name != "CampUpgradesPanel")
                        child.gameObject.SetActive(false);

                }
            }
            gameObject.SetActive(false);
            panelScript.DeactivateSpotligths();
            panelScript.DeactivateNewSoldiers();
        }
       
    }

}
 