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
    class WeaponGenerationEditor : EditorWindow
    {
        public WeaponGenerator prefabScript;

        [MenuItem("Tweaks/Weapon Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            WeaponGenerationEditor window = (WeaponGenerationEditor)EditorWindow.GetWindow(typeof(WeaponGenerationEditor));
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
                    prefabScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<WeaponGenerator>();

                    GUILayout.Label("General", EditorStyles.boldLabel);
                    prefabScript.newWeaponsNumber = EditorGUILayout.IntField(prefabScript.newWeaponsNumber);
                    prefabScript.increasePointsMultiplier = EditorGUILayout.IntField(prefabScript.increasePointsMultiplier);
                    prefabScript.increasePointsAdder = EditorGUILayout.IntField(prefabScript.increasePointsAdder);

                    GUILayout.Label("Shield settings", EditorStyles.boldLabel);
                    prefabScript.damagePercentagePolearm = EditorGUILayout.Slider(prefabScript.damagePercentagePolearm, 0.0f, 1.0f);
                    prefabScript.damageSpeedPercentagePolearm = EditorGUILayout.Slider(prefabScript.damageSpeedPercentagePolearm, 0.0f, 1.0f);
                    prefabScript.healthPercentagePolearm = EditorGUILayout.Slider(prefabScript.healthPercentagePolearm, 0.0f, 1.0f);
                    prefabScript.polearmRange = EditorGUILayout.IntField(prefabScript.polearmRange);

                    GUILayout.Label("Polearm settings", EditorStyles.boldLabel);
                    prefabScript.damagePercentagePolearm = EditorGUILayout.Slider(prefabScript.damagePercentagePolearm, 0.0f, 1.0f);
                    prefabScript.damageSpeedPercentagePolearm = EditorGUILayout.Slider(prefabScript.damageSpeedPercentagePolearm, 0.0f, 1.0f);
                    prefabScript.healthPercentagePolearm = EditorGUILayout.Slider(prefabScript.healthPercentagePolearm, 0.0f, 1.0f);
                    prefabScript.polearmRange = EditorGUILayout.IntField(prefabScript.polearmRange);

                    GUILayout.Label("Rifle settings", EditorStyles.boldLabel);
                    prefabScript.damagePercentagePolearm = EditorGUILayout.Slider(prefabScript.damagePercentagePolearm, 0.0f, 1.0f);
                    prefabScript.damageSpeedPercentagePolearm = EditorGUILayout.Slider(prefabScript.damageSpeedPercentagePolearm, 0.0f, 1.0f);
                    prefabScript.healthPercentagePolearm = EditorGUILayout.Slider(prefabScript.healthPercentagePolearm, 0.0f, 1.0f);
                    prefabScript.polearmRange = EditorGUILayout.IntField(prefabScript.polearmRange);
                }
                catch (Exception e) {
                    GUILayout.Label("The prefab is messed up, go to Peter!", EditorStyles.boldLabel);
                }
            }
        }

    }
}
