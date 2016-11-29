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
    class LootboxEditor : EditorWindow
    {
        public LootboxScript prefabScript;

        [MenuItem("Tweaks/Lootbox Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            LootboxEditor window = (LootboxEditor)EditorWindow.GetWindow(typeof(LootboxEditor));
            window.Show();
        }

        void OnGUI()
        {
            this.titleContent = new GUIContent("Lootboxes");
            string[] guids = AssetDatabase.FindAssets("t:Prefab Lootbox");

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
                    prefabScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<LootboxScript>();

                    GUILayout.Label("General", EditorStyles.boldLabel);
                    prefabScript.ProbabilityOfSpawn = EditorGUILayout.Slider("Probability of spawn",prefabScript.ProbabilityOfSpawn,0.0f,1.0f);
                    prefabScript.MinimumScrap = EditorGUILayout.IntField("Minimum value in box", prefabScript.MinimumScrap);
                    prefabScript.MaxScrap = EditorGUILayout.IntField("Max value in box", prefabScript.MaxScrap);
                    prefabScript.ScaleByLevel = EditorGUILayout.FloatField("Min/max scale factor", prefabScript.ScaleByLevel);
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
