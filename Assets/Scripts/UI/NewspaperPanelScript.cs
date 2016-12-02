using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewspaperPanelScript : MonoBehaviour {

    public Image NewspaperImage;
    public GameObject ContentPanel;

    public void SetNewspaperImage(Sprite newspaper)
    {
		if (!GameObject.Find("NewspaperPanel").transform.FindChild("NewspaperPanelContent").gameObject.activeSelf)
		{
			EventManager.Instance.TriggerEvent(new UIPanelActiveEvent());
		}
		NewspaperImage.sprite = newspaper;
        ContentPanel.SetActive(true);
	}

	public void ClosePanel()
	{
        Manager_Audio.ChangeState(Manager_Audio.commandWheelContainer, Manager_Audio.closeWheel);
        EventManager.Instance.TriggerEvent(new UIPanelActiveEvent());
	}
}
