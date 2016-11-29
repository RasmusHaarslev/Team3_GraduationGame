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
            string[] guids = AssetDatabase.FindAssets("t:Prefab LevelSelectionGenerator");

            if (guids.Length == 0)
            {
                GUILayout.Label("The prefab is missing, go to Peter!", EditorStyles.boldLabel);
            }
            else if (guids.Length > 1)
            {
                GUILayout.Label("More than one prefabs with same name, go to Peter!", EditorStyles.boldLabel);
            }
            else
            {
                try
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids.FirstOrDefault());
                    prefabScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<LevelSelectionGenerator>();

                    GUILayout.Label("General", EditorStyles.boldLabel);
                    prefabScript.amountOfRows = EditorGUILayout.IntField("Amount of rows generated",prefabScript.amountOfRows);

                    GUILayout.Label("Item Drops", EditorStyles.boldLabel);
                    prefabScript.MaxItemDropAmount = EditorGUILayout.IntSlider("Max Amount of items dropped", prefabScript.MaxItemDropAmount, 2, 5);

                    GUILayout.Label("Lowest Travel Cost", EditorStyles.boldLabel);
                    prefabScript.LowestTravelCost = EditorGUILayout.IntSlider("Minimum amount of food it takes to Travel", prefabScript.LowestTravelCost, 0, 3);

                    GUILayout.Label("Highest Travel Cost", EditorStyles.boldLabel);
                    prefabScript.MaxItemDropAmount = EditorGUILayout.IntSlider("Highest amount of food it takes to Travel", prefabScript.HighestTravelCost, prefabScript.LowestTravelCost, 6);

                    GUILayout.Label("Lowest Scout Cost", EditorStyles.boldLabel);
                    prefabScript.LowestScoutCost = EditorGUILayout.IntSlider("Minimum amount of food it takes to Scout", prefabScript.LowestScoutCost, 0, 3);

                    GUILayout.Label("Highest Scout Cost", EditorStyles.boldLabel);
                    prefabScript.HighestScoutCost = EditorGUILayout.IntSlider("Highest amount of food it takes to Scout", prefabScript.HighestScoutCost, prefabScript.LowestScoutCost, 6);

                    if (GUILayout.Button("Save"))
                    {
                        var go = Instantiate(prefabScript.gameObject);
                        PrefabUtility.ReplacePrefab(go, prefabScript.gameObject, ReplacePrefabOptions.ReplaceNameBased);
                        DestroyImmediate(go);
                    }
                }
                catch (Exception e) {
                    GUILayout.Label("The prefab is messed up, go to Peter!", EditorStyles.boldLabel);
                }
            }
        }

    }
}
