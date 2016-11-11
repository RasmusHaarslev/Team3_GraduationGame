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
    class NetworkWindow : EditorWindow
    {
        [MenuItem("Window/Lock Inspector #&w")]
        static void LockEditor()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        }

        [MenuItem("Level Design/Random Y-axis rotation %q")]
        static void RandomYRotation()
        {
            foreach (var go in Selection.gameObjects)
            {
                go.transform.Rotate(new Vector3(0, Random.Range(0.0f, 360.0f), 0));

                if(Random.Range(0, 2) == 0)
                    go.transform.localScale = new Vector3(go.transform.localScale.x * -1, go.transform.localScale.y, go.transform.localScale.z);
                else
                    go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
            }
            
        }

        [MenuItem("Level Design/Random rotation %#q")]
        static void RandomRotation()
        {
            foreach (var go in Selection.gameObjects)
            {
                go.transform.Rotate(new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));

                if (Random.Range(0, 2) == 0)
                    go.transform.localScale = new Vector3(go.transform.localScale.x * -1, go.transform.localScale.y, go.transform.localScale.z);
                else
                    go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
            }
        }

        [MenuItem("Level Design/Random rotation %w")]
        static void RandomScale()
        {
            foreach (var go in Selection.gameObjects)
            {
                go.transform.localScale = new Vector3(go.transform.localScale.x * Random.Range(0.9f,1.1f), go.transform.localScale.y * Random.Range(0.8f, 1.2f), go.transform.localScale.z * Random.Range(0.9f, 1.1f));
            }

        }
    }
}
