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
    class UpgradesEditor : EditorWindow
    {
        public CampManager prefabScript;

        [MenuItem("Tweaks/Upgrades Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            UpgradesEditor window = (UpgradesEditor)EditorWindow.GetWindow(typeof(UpgradesEditor));
            window.Show();
        }

        void OnGUI()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab CampUpgradesPanel");

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
                    prefabScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<CampManager>();

                    GUILayout.Label("Basic upgrades and increase pr level", EditorStyles.boldLabel);

                    prefabScript.GatherCost = EditorGUILayout.IntField("Gather cost", prefabScript.GatherCost);
                    prefabScript.GatherCostIncrease = EditorGUILayout.IntField("Gather cost increase", prefabScript.GatherCostIncrease);

                    prefabScript.MaxVillagesCost = EditorGUILayout.IntField("Max villages cost", prefabScript.MaxVillagesCost);
                    prefabScript.MaxVillagesCostIncrease = EditorGUILayout.IntField("Max villages cost increase", prefabScript.MaxVillagesCostIncrease);

                    prefabScript.LeaderHealthCost = EditorGUILayout.IntField("Leader health cost", prefabScript.LeaderHealthCost);
                    prefabScript.LeaderHealthCostIncrease = EditorGUILayout.IntField("Leader health cost increase", prefabScript.LeaderHealthCostIncrease);

                    prefabScript.LeaderStrengthCost = EditorGUILayout.IntField("Leader strength cost", prefabScript.LeaderStrengthCost);
                    prefabScript.LeaderStrengthCostIncrease = EditorGUILayout.IntField("Leader strength cost increase", prefabScript.LeaderStrengthCostIncrease);

                    GUILayout.Label("Finish now cost", EditorStyles.boldLabel);
                    prefabScript.FinishUpgradeCost = EditorGUILayout.IntField("Gold for using finish now", prefabScript.FinishUpgradeCost);

                    GUILayout.Label("Editor for camp upgrades", EditorStyles.boldLabel);
                    prefabScript.Level1_Time = EditorGUILayout.IntField("Seconds for level 1", prefabScript.Level1_Time);
                    prefabScript.Level2_Time = EditorGUILayout.IntField("Seconds for level 2", prefabScript.Level2_Time);
                    prefabScript.Level3_Time = EditorGUILayout.IntField("Seconds for level 3", prefabScript.Level3_Time);
                    prefabScript.Level4_Time = EditorGUILayout.IntField("Seconds for level 4", prefabScript.Level4_Time);
                    prefabScript.Level5_Time = EditorGUILayout.IntField("Seconds for level 5", prefabScript.Level5_Time);
                    prefabScript.Level6_Time = EditorGUILayout.IntField("Seconds for level 6", prefabScript.Level6_Time);
                    prefabScript.Level7_Time = EditorGUILayout.IntField("Seconds for level 7", prefabScript.Level7_Time);
                    prefabScript.Level8_Time = EditorGUILayout.IntField("Seconds for level 8", prefabScript.Level8_Time);
                    prefabScript.Level9_Above_Time = EditorGUILayout.IntField("Seconds for level 9+", prefabScript.Level9_Above_Time);
                }
                catch (Exception e)
                {
                    GUILayout.Label("The prefab is messed up, go to Peter!", EditorStyles.boldLabel);
                }
            }


        }

    }
}
