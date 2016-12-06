using System;
using System.Collections;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

public class PaletteApplier : MonoBehaviour
{
    [Header("Options"), Tooltip("Amount of rows between palette switch")]
    public int PaletteInterval = 2;
    [Tooltip("Override palette")]
    public bool OverridePalette = false;
    [Tooltip("Choose the palette specifically for this level")]
    public int CustomPalette = 2;

    [Header("Materials to change")]
    public Material walkableGroundMaterial;
    public Material nonWalkableGroundMaterial;
    private int paletteNumber = 1;

    //Declare colors
    private Color ambientSkyColor;
    private Color ambientEquatorColor;
    private Color ambientGroundColor;

    private Color walkableGroundColor;
    private Color nonWalkableGroundColor;

    void OnEnable()
    {
        EventManager.Instance.StartListening<LevelStarted>(ChangePalette);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<LevelStarted>(ChangePalette);
    }

    void OnApplicationExit()
    {
        this.gameObject.SetActive(false);
    }

    public void ChangePalette(LevelStarted e)
    {
        paletteNumber = PlayerPrefs.GetInt(StringResources.LevelDifficultyPrefsName);
        paletteNumber = ((int)(paletteNumber * (1.0f / (float)PaletteInterval))) % 3;

        if (OverridePalette)
        {
            paletteNumber = CustomPalette;
        }

        //Parse colors
        switch (paletteNumber)
        {
            default:
            case 0: //Blueish palette | visible in regular levels as 1st "world"
                {
                    parseColors("#87CFFFFF", "#385C5FFF", "#FFFFFF", "#BDCDFF", "#0007FF");
                }
                Debug.Log("Color palette 1");
                Manager_Audio.ChangeState("Palette", "Color3");
                break;
            case 1: //Yellow palette | visible in tutorial 1-3 and in regular levels as 2nd "world"
                {
                    parseColors("#E9C57EFF", "#7D8852FF", "#F6FFD0", "#F4FF52", "#FF4B4B");
                }
                Debug.Log("Color palette 2");
                Manager_Audio.ChangeState("Palette", "Color1");
                break;
            case 2: //Red/purple palette | visible in tutorial 4-5 and in regular levels as 3rd "world"
                {
                    parseColors("#CBB7FFFF", "#5B685FFF", "#FFFFFF", "#FFAEAE", "#FFCA00");
                }
                Debug.Log("Color palette 3");
                Manager_Audio.ChangeState("Palette", "Color2");
                break;
        }

        //Apply colors
        RenderSettings.ambientSkyColor = ambientSkyColor;
        RenderSettings.ambientEquatorColor = ambientEquatorColor;
        RenderSettings.ambientGroundColor = ambientGroundColor;

        walkableGroundMaterial.SetColor("_MainColor", walkableGroundColor);
        nonWalkableGroundMaterial.SetColor("_MainColor", nonWalkableGroundColor);
    }

    //Collectively parses all colors in a palette, and sends them to appropriate
    void parseColors(string c1, string c2, string c3, string c4, string c5)
    {
        ColorUtility.TryParseHtmlString(c1, out walkableGroundColor);
        ColorUtility.TryParseHtmlString(c2, out nonWalkableGroundColor);

        ColorUtility.TryParseHtmlString(c3, out ambientSkyColor);
        ColorUtility.TryParseHtmlString(c4, out ambientEquatorColor);
        ColorUtility.TryParseHtmlString(c5, out ambientGroundColor);
    }

}
