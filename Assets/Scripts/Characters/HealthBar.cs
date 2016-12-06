using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class HealthBar : MonoBehaviour
{
	public float barDisplay;
	Vector3 pos;
	public Vector2 size = new Vector2(120, 15);
	Texture2D progressBarEmpty;
	Texture2D progressBarFull;
	Character character;
	Camera camera;
	Vector3 screenPos;
	GUIStyle currentStyle = null;
	public bool active = true;

	// Use this for initialization
	void Start()
	{
		character = transform.parent.gameObject.GetComponent<Character>();
		camera = Camera.main;
		progressBarFull = new Texture2D((int)size.x, (int)size.y);
		for (int y = 0; y < progressBarFull.height; ++y)
		{
			for (int x = 0; x < progressBarFull.width; ++x)
			{
				Color color = Color.white;
				progressBarFull.SetPixel(x, y, color);
			}
		}
		progressBarFull.Apply();
		screenPos = camera.WorldToScreenPoint(transform.position);
		pos = screenPos;
		barDisplay = character.currentHealth / character.health;
	}

	void OnEnable()
	{
		EventManager.Instance.StartListening<UIPanelActiveEvent>(ActivateHealthPanel);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<UIPanelActiveEvent>(ActivateHealthPanel);
	}


	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	private void ActivateHealthPanel(UIPanelActiveEvent e)
	{
		active = e.panelActive;
	}

	// Update is called once per frame
	void Update()
	{
		if (currentStyle == null)
		{
			currentStyle = new GUIStyle();
		}
		screenPos = camera.WorldToScreenPoint(transform.position);
		pos = new Vector2(screenPos.x, screenPos.y + 100);
		barDisplay = character.currentHealth / character.health;
	}

	void OnGUI()
	{
		if (active)
		{
			// draw the background:
			if (SceneManager.GetActiveScene().name != "CampManagement" && !character.isFleeing && !character.isDead)
			{
				if (transform.parent.gameObject.tag == "Friendly" || transform.parent.gameObject.tag == "Player")
				{
					if (barDisplay <= 0.25)
					{
						GUI.color = new Color((float)159 / 255, (float)33 / 255, (float)33 / 255);
					}
					else
					if (barDisplay <= 0.5)
					{
						GUI.color = new Color((float)187 / 255, (float)99 / 255, (float)35 / 255);
					}
					else
					if (barDisplay <= 1.0)
					{
						GUI.color = new Color((float)89 / 255, (float)122 / 255, (float)22 / 255);
					}
				}
				else
				{
					if (barDisplay <= 0.25)
					{
						GUI.color = new Color((float)185 / 255, (float)66 / 255, (float)167 / 255);
					}
					else
					if (barDisplay <= 0.5)
					{
						GUI.color = new Color((float)127 / 255, (float)65 / 255, (float)190 / 255);
					}
					else
					if (barDisplay <= 1.0)
					{
						GUI.color = new Color((float)27 / 255, (float)70 / 255, (float)158 / 255);
					}
				}
				currentStyle.normal.background = progressBarFull;
				GUI.BeginGroup(new Rect(pos.x - size.x / 2, Screen.height - pos.y, size.x, size.y));
				GUI.Box(new Rect(0, 0, size.x, size.y), progressBarEmpty);

				// draw the filled-in part:
				GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
				GUI.Box(new Rect(0, 0, size.x, size.y), progressBarFull, currentStyle);
				GUI.EndGroup();
				GUI.EndGroup();
			}
		}
	}
}
