using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OptionsPanel : MonoBehaviour
{

	public GameObject OptionPanel = null;
	public GameObject BackgroundPanel;

	public void TogglePanelOpen()
	{
		int scene = SceneManager.GetActiveScene().buildIndex;

		if (scene != 2)
		{
			Time.timeScale = Time.timeScale == 1 ? 0 : 1;
		}

		Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
		if (GameController.Instance.numberOfActiveUIs == 0)
		{
			EventManager.Instance.TriggerEvent(new UIPanelActiveEvent(false));
		}
		GameController.Instance.numberOfActiveUIs++;
		BackgroundPanel.SetActive(true);
		OptionPanel.SetActive(true);
	}

}