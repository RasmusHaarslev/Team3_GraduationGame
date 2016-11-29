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
    class CharacterEditor : EditorWindow
    {
        public CampManager prefabScript;

        [MenuItem("Tweaks/Character Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            CharacterEditor window = (CharacterEditor)EditorWindow.GetWindow(typeof(CharacterEditor));
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

                    GUILayout.Label("Character stuff coming", EditorStyles.boldLabel);
                    if (GUILayout.Button("Save"))
                    {
                        var go = Instantiate(prefabScript.gameObject);
                        PrefabUtility.ReplacePrefab(go, prefabScript.gameObject, ReplacePrefabOptions.ReplaceNameBased);
                        DestroyImmediate(go);
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
