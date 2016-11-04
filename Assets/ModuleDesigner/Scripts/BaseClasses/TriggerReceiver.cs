using System;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts.BaseClasses
{
    public abstract class TriggerReceiver : MonoBehaviour
    {
        [Header("Gizmo options")]
        [Tooltip("Keep gizmo visible")]
        public Boolean KeepGizmo = true;

        // override these
        public abstract void TriggerEnter();
        public abstract void TriggerExit();
        public abstract void Expose(GameObject go);
        public abstract void ShowGizmos();
    }
}
