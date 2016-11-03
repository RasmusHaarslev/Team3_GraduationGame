using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    class NetworkWindow : EditorWindow
    {
        [MenuItem("Window/Lock Inspector #&w")]
        static void LockEditor()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        }
    }
}
