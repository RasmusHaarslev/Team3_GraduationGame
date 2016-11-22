using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Editor
{
    class LevelSelectionEditor : EditorWindow
    {
        public LevelSelectionGenerator prefabScript;

        [MenuItem("Tweaks/Level Selection Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            LevelSelectionEditor window = (LevelSelectionEditor)EditorWindow.GetWindow(typeof(LevelSelectionEditor));
            window.Show();
        }

        void OnGUI()
        {
            string[] guids = AssetDatabase.FindAssets("LevelGenerator");

            if (guids.Length == 0)
            {
                GUILayout.Label("The prefab is missing, go to Peter!", EditorStyles.boldLabel);
            }
            else
            {
                try
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids.FirstOrDefault());
                    prefabScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<LevelSelectionGenerator>();

                    GUILayout.Label("General", EditorStyles.boldLabel);
                    prefabScript.amountOfRows = EditorGUILayout.IntField(prefabScript.amountOfRows, "Initial amount of rows");

                    GUILayout.Label("Probabilities", EditorStyles.boldLabel);
                    prefabScript.probabilityTribes = EditorGUILayout.IntSlider(prefabScript.probabilityTribes, 0, 5);

                }
                catch (Exception e) {
                    GUILayout.Label("The prefab is messed up, go to Peter!", EditorStyles.boldLabel);
                }
            }
        }

    }
}
