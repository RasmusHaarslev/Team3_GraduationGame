using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewspaperPanelScript : MonoBehaviour {

    public Image NewspaperImage;
    public GameObject ContentPanel;

    public void SetNewspaperImage(Sprite newspaper)
    {
        if (!ContentPanel.activeSelf)
        {
            Manager_Audio.PlaySound("Play_PaperPickup", this.gameObject);
            Manager_Audio.ChangeState(Manager_Audio.commandWheelContainer, Manager_Audio.openWheel);

            if (!GameObject.Find("NewspaperPanel").transform.FindChild("NewspaperPanelContent").gameObject.activeSelf)
            {
                EventManager.Instance.TriggerEvent(new UIPanelActiveEvent(true));
            }
            NewspaperImage.sprite = newspaper;
            ContentPanel.SetActive(true);
        }
	}

	public void ClosePanel()
	{
        Manager_Audio.ChangeState(Manager_Audio.commandWheelContainer, Manager_Audio.closeWheel);
        EventManager.Instance.TriggerEvent(new UIPanelActiveEvent(false));
	}
}
