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
				if (GameController.Instance.numberOfActiveUIs == 0)
				{
					EventManager.Instance.TriggerEvent(new UIPanelActiveEvent(false));
				}
				GameController.Instance.numberOfActiveUIs++;
			}
            NewspaperImage.sprite = newspaper;
            ContentPanel.SetActive(true);
        }
	}

	public void ClosePanel()
	{
        Manager_Audio.ChangeState(Manager_Audio.commandWheelContainer, Manager_Audio.closeWheel);
		if (GameController.Instance.numberOfActiveUIs == 1)
		{
			EventManager.Instance.TriggerEvent(new UIPanelActiveEvent(true));
		}
		GameController.Instance.numberOfActiveUIs--;
	}

	public void ContinueTime()
	{
		Time.timeScale = 1;
	}
}
