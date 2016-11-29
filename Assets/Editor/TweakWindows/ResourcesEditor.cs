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
    class ResourcesEditor : EditorWindow
    {
        public GameController gameControllerScript;
        public LevelSelectionGenerator levelGeneratorScript;

        [MenuItem("Tweaks/Resources Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            ResourcesEditor window = (ResourcesEditor)EditorWindow.GetWindow(typeof(ResourcesEditor));
            window.Show();
        }

        void OnGUI()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab GameController");
            string[] guids2 = AssetDatabase.FindAssets("t:Prefab LevelGenerator");

            if (guids.Length == 0 || guids2.Length == 0)
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
                    gameControllerScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<GameController>();

                    string path2 = AssetDatabase.GUIDToAssetPath(guids2.FirstOrDefault());
                    levelGeneratorScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<LevelSelectionGenerator>();

                    GUILayout.Label("Initial resources", EditorStyles.boldLabel);
                    gameControllerScript.InitialFood = EditorGUILayout.IntField("Initial food", gameControllerScript.InitialFood);
                    gameControllerScript.InitialVillages = EditorGUILayout.IntField("Initial villages", gameControllerScript.InitialVillages);
                    gameControllerScript.InitialScrap = EditorGUILayout.IntField("Initial scrap", gameControllerScript.InitialScrap);
                    gameControllerScript.InitialPremium = EditorGUILayout.IntField("Initial premium", gameControllerScript.InitialPremium);

                    GUILayout.Label("More to come...", EditorStyles.boldLabel);

                    if (GUILayout.Button("Save"))
                    {
                        var go = Instantiate(prefabScript.gameObject);
                        PrefabUtility.ReplacePrefab(go, prefabScript.gameObject, ReplacePrefabOptions.ReplaceNameBased);
                        Destroy(go);
                    }
                }
                catch (Exception e)
                {
                    GUILayout.Label("The prefab is messed up, go to Peter!", EditorStyles.boldLabel);
                }
            }

        }

    }
}
