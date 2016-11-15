﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class DebugUI : MonoBehaviour
{
	private bool expandLog = false;
	private bool frozen = false;

	#region Trait debugging
	private bool follower1 = false;
	private bool follower2 = false;
	private bool follower3 = false;

	public List<GameObject> followers = new List<GameObject>();
	#endregion

	public string output = "";

	private static DebugUI s_Instance = null;
	public static DebugUI Instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = GameObject.FindObjectOfType(typeof(DebugUI)) as DebugUI;
			}
			return s_Instance;
		}
	}

	void Start()
	{
	}

	void OnEnable()
	{

		Application.logMessageReceived += HandleLog;
	}




	}





	void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (type.ToString() == "Exception")
		{
			output += logString + "\n" + stackTrace + "\n";
			SendMail();
		}
	}

	void Update()
	{
		if (SceneManager.GetActiveScene().name == "LevelPrototype01")
		{
			if (followers.Count < 3 || followers[0] == null || followers[1] == null || followers[2] == null)
			{
				followers.Clear();
				followers.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
			}
		}
	}

	void OnGUI()
	{
		GUIStyle customButton = new GUIStyle("button");
		customButton.fontSize = 35;

		GUIStyle customLabel = new GUIStyle("label");
		customLabel.fontSize = 35;
		customLabel.fontStyle = FontStyle.Bold;
		customLabel.normal.textColor = Color.magenta;

		GUIStyle customDebug = new GUIStyle("button");
		customDebug.alignment = TextAnchor.UpperLeft;
		customDebug.fontSize = 30;
		customDebug.normal.textColor = Color.white;



		float height = 100;
		float xPosLeft = 10;
		float xPosRight = Screen.width - width - 10;
		float yPosLeft = 10;
		float yPosRight = 10;




					}




					}







		{
			if (expandLog)
			{
				yPosLeft = 10;
				if (GUI.Button(new Rect(xPosLeft, yPosLeft, width, height), "Close log", customButton))
					expandLog = false;

				if (GUI.Button(new Rect(xPosLeft, yPosLeft, width, height), "Clear log", customButton))
					output = "";

					SendMail();

				GUI.Label(new Rect(10, 120, 2028, 1406), "Debug:", customLabel);
				GUI.Label(new Rect(10, 170, 2028, 1406), output, customDebug);
			}
			else
			{

				yPosLeft = 10;
				if (GUI.Button(new Rect(xPosLeft, yPosLeft, width, height), "Close", customButton))
					showDebug = false;

				if (GUI.Button(new Rect(xPosRight, yPosRight, width, height), "Gameplay", customButton))
				{
					followers.Clear();
					SceneManager.LoadScene("LevelPrototype");
				}

				yPosRight += height + 10;
				if (GUI.Button(new Rect(xPosRight, yPosRight, width, height), "To camp", customButton))
				{
					followers.Clear();
					SceneManager.LoadScene("CampManagement");
				}

				yPosRight += height + 10;
				if (GUI.Button(new Rect(xPosRight, yPosRight, width, height), "Level \nselection", customButton))
				{
					followers.Clear();
					SceneManager.LoadScene("LevelSelection");
				}


				yPosRight += height + 10;
				if (GUI.Button(new Rect(xPosRight, yPosRight, width, height), "Demo Level", customButton))
				{
					followers.Clear();
					SceneManager.LoadScene("DemoLevel");
				}

				yPosRight += height + 10;
				if (GUI.Button(new Rect(xPosRight, yPosRight, width, height), "Win level", customButton))
				{
					GameObject.Find("LevelGenerator").GetComponent<LevelManager>().WinLevel();
				}


                yPosRight += height + 10;
                if (SceneManager.GetActiveScene().name == "LevelSelection") { 
                    if (GUI.Button(new Rect(xPosRight, yPosRight, width, height), "Export levels", customButton))
                        SaveLoadLevels.SaveLevels(GameObject.Find("LevelGenerator").GetComponent<LevelSelectionGenerator>().nodesInRows);
                }


				yPosRight += height + 50;
				if (GUI.Button(new Rect(Screen.width / 2 - width / 2, 10, width, height), "Trait \nmanagement", customButton))
					traitManagement = true;
				#endregion

				if (frozen)
				{
					if (GUI.Button(new Rect(xPosLeft, yPosLeft, width, height), "Unfreeze", customButton))
					{
						Time.timeScale = 1f;
						frozen = false;
					}
				}
				else
				{
					if (GUI.Button(new Rect(xPosLeft, yPosLeft, width, height), "Freeze", customButton))
					{
						Time.timeScale = Mathf.Epsilon;
						frozen = true;
					}
				}
				#endregion

				yPosLeft += height + 10;
				if (GUI.Button(new Rect(xPosLeft, yPosLeft, width, height), "Screenshot", customButton))
				{
					string Name = "Screenshot_" + DateTime.Now.ToString("HH.mm.ss___yyyy-MM-dd") + ".png";
					Application.CaptureScreenshot(Name);
				}

				yPosLeft += height + 10;
				if (GUI.Button(new Rect(xPosLeft, yPosLeft, width, height), "Expand log", customButton))
				{
					expandLog = true;
				}

				yPosLeft += height + 10;
				if (GUI.Button(new Rect(xPosLeft, yPosLeft, width, height), "Send log", customButton))
					SendMail();

				GUI.Label(new Rect(width + 25, 20, 400, height), "Current scene: " + SceneManager.GetActiveScene().name, customLabel);

				GUI.Label(new Rect(10, 976, 200, 50), "Debug:", customLabel);
				GUI.Label(new Rect(10, 1026, 2028, 500), output, customDebug);
			}
		}
		else
		{
			yPosLeft = 10;
			if (GUI.Button(new Rect(xPosLeft, yPosLeft, width - 50, height - 50), "Debug", customButton))
				showDebug = true;
		}
	}

	private void SendMail()
	{
		MailMessage mail = new MailMessage();

		mail.From = new MailAddress("dadiuteam3tester@gmail.com");
		mail.To.Add("selech07@gmail.com");
		mail.Subject = "Debug mail";
		mail.Body = "Debug log \n" + output;

		SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential("dadiuteam3tester@gmail.com", "team3builder") as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		ServicePointManager.ServerCertificateValidationCallback =
			delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{ return true; };
		smtpServer.Send(mail);
		Debug.Log("Mail send!");
	}

}
