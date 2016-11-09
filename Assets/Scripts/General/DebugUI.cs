using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class DebugUI : MonoBehaviour
{

    private bool showDebug = false;
    private bool expandLog = false;
    private bool frozen = false;

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
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type.ToString() == "Exception")
        {
            output += logString + "\n" + stackTrace + "\n";
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

        float width = 200;
        float height = 100;
        float xPos = 10;
        float yPos = 10;

        if (showDebug)
        {
            if (expandLog)
            {
                yPos = 10;
                if (GUI.Button(new Rect(xPos, yPos, width, height), "Close log", customButton))
                    expandLog = false;

                if (GUI.Button(new Rect(220, yPos, width, height), "Clear log", customButton))
                    output = "";

                GUI.Label(new Rect(10, 120, 2028, 1406), "Debug:", customLabel);
                GUI.Label(new Rect(10, 170, 2028, 1406), output, customDebug);
            }
            else
            {

                yPos = 10;
                if (GUI.Button(new Rect(xPos, yPos, width, height), "Close", customButton))
                    showDebug = false;

                yPos += height + 10;
                if (GUI.Button(new Rect(xPos, yPos, width, height), "Reload", customButton))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                #region Freeze
                yPos += height + 10;
                if (frozen)
                {
                    if (GUI.Button(new Rect(xPos, yPos, width, height), "Unfreeze", customButton))
                    {
                        Time.timeScale = 1f;
                        frozen = false;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(xPos, yPos, width, height), "Freeze", customButton))
                    {
                        Time.timeScale = Mathf.Epsilon;
                        frozen = true;
                    }
                }
                #endregion

                yPos += height + 10;
                if (GUI.Button(new Rect(xPos, yPos, width, height), "Screenshot", customButton))
                {
                    string Name = "Screenshot_" + DateTime.Now.ToString("HH.mm.ss___yyyy-MM-dd") + ".png";
                    Application.CaptureScreenshot(Name);
                }

                yPos += height + 10;
                if (GUI.Button(new Rect(xPos, yPos, width, height), "Expand log", customButton))
                {
                    expandLog = true;
                }

                GUI.Label(new Rect(225, 20, 400, height), "Current scene: " + SceneManager.GetActiveScene().name, customLabel);

                GUI.Label(new Rect(10, 976, 200, 50), "Debug:", customLabel);
                GUI.Label(new Rect(10, 1026, 2028, 500), output, customDebug);
            }
        }
        else
        {
            yPos = 10;
            if (GUI.Button(new Rect(xPos, yPos, width - 50, height - 50), "Debug", customButton))
                showDebug = true;
        }
    }
}
