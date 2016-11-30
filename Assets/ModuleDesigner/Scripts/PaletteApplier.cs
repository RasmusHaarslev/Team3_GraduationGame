using System;
using System.Collections;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class PaletteApplier : MonoBehaviour
    {
        [Header("Options"),Tooltip("Amount of rows between palette switch")]
        public int PaletteInterval = 2;

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

        public void ChangePalette(LevelStarted e)
        {
            paletteNumber = PlayerPrefs.GetInt(StringResources.LevelDifficultyPrefsName);
            paletteNumber = ((int)(paletteNumber * (1.0f/(float) PaletteInterval))) % 3;

            //Parse colors
            switch (paletteNumber)
            {
                default:
                case 0: //Yellow palette
                    {
                        parseColors("#D7D1B4", "#A5A08A", "#647388", "#5F4B03", "#472A00");
                    }
                    Manager_Audio.ChangeState("Palette","Color1");
                    break;
                case 1: //Red/purple palette
                    {
                        parseColors("#AE9393", "#867171", "#848AA1", "#434E88", "#3F3D4C");
                    }
                    Manager_Audio.ChangeState("Palette", "Color2");
                    break;
                case 2: 
                    {
                        parseColors("#AE9393", "#867171", "#848AA1", "#434E88", "#3F3D4C");
                    }
                    Manager_Audio.ChangeState("Palette", "Color3");
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
}
