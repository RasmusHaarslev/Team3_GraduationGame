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
    class RivalsEditor : EditorWindow
    {
        public RivalStateMachine prefabScript;

        [MenuItem("Tweaks/Rivals Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            RivalsEditor window = (RivalsEditor)EditorWindow.GetWindow(typeof(RivalsEditor));
            window.Show();
        }

        void OnGUI()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab Rival");

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
                    prefabScript = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<RivalStateMachine>();

                    GUILayout.Label("General", EditorStyles.boldLabel);
                    prefabScript.fleeHealthLimit = EditorGUILayout.FloatField("Flee health limit", prefabScript.fleeHealthLimit);

                    GUILayout.Label("Maybe more?...", EditorStyles.boldLabel);
                }
                catch (Exception e) {
                    GUILayout.Label("The prefab is messed up, go to Peter!", EditorStyles.boldLabel);
                }
            }
        }

    }
}
