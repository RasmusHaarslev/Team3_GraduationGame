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
            string[] guids = AssetDatabase.FindAssets("t:Prefab LevelGenerator");

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
                    prefabScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<WeaponGenerator>();

                    GUILayout.Label("General", EditorStyles.boldLabel);
                    prefabScript.newWeaponsNumber = EditorGUILayout.IntField("Amount of new weapons", prefabScript.newWeaponsNumber);
                    prefabScript.increasePointsMultiplier = EditorGUILayout.IntField("Points multiplier per level", prefabScript.increasePointsMultiplier);
                    prefabScript.increasePointsAdder = EditorGUILayout.IntField("Points added", prefabScript.increasePointsAdder);

                    GUILayout.Label("Shield settings", EditorStyles.boldLabel);
                    prefabScript.damagePercentageShield = EditorGUILayout.Slider("Damage percentage", prefabScript.damagePercentageShield, 0.0f, 1.0f);
                    prefabScript.damageSpeedPercentageShield = EditorGUILayout.Slider("Speed percentage", prefabScript.damageSpeedPercentageShield, 0.0f, 1.0f);
                    prefabScript.healthPercentageShield = EditorGUILayout.Slider("Health percentage", prefabScript.healthPercentageShield, 0.0f, 1.0f);
                    prefabScript.shieldRange = EditorGUILayout.IntField("Range", prefabScript.shieldRange);

                    GUILayout.Label("Polearm settings", EditorStyles.boldLabel);
                    prefabScript.damagePercentagePolearm = EditorGUILayout.Slider("Damage percentage", prefabScript.damagePercentagePolearm, 0.0f, 1.0f);
                    prefabScript.damageSpeedPercentagePolearm = EditorGUILayout.Slider("Speed percentage", prefabScript.damageSpeedPercentagePolearm, 0.0f, 1.0f);
                    prefabScript.healthPercentagePolearm = EditorGUILayout.Slider("Health percentage", prefabScript.healthPercentagePolearm, 0.0f, 1.0f);
                    prefabScript.polearmRange = EditorGUILayout.IntField("Range", prefabScript.polearmRange);

                    GUILayout.Label("Rifle settings", EditorStyles.boldLabel);
                    prefabScript.damagePercentageRifle = EditorGUILayout.Slider("Damage percentage", prefabScript.damagePercentageRifle, 0.0f, 1.0f);
                    prefabScript.damageSpeedPercentageRifle = EditorGUILayout.Slider("Speed percentage", prefabScript.damageSpeedPercentageRifle, 0.0f, 1.0f);
                    prefabScript.healthPercentageRifle = EditorGUILayout.Slider("Health percentage", prefabScript.healthPercentageRifle, 0.0f, 1.0f);
                    prefabScript.rifleRange = EditorGUILayout.IntField("Range", prefabScript.rifleRange);

                    if (GUILayout.Button("Save"))
                    {
                        var go = Instantiate(prefabScript.gameObject);
                        PrefabUtility.ReplacePrefab(go, prefabScript.gameObject, ReplacePrefabOptions.ReplaceNameBased);
                        Destroy(go);
                    }
                }
                catch (Exception e) {
                    GUILayout.Label("The prefab is messed up, go to Peter!", EditorStyles.boldLabel);
                }
            }
        }

    }
}
