﻿using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class WwiseEvent : TriggerReceiver {

        [Header("Wwise events")]
        [Tooltip("Wwise event triggered on enter")]
        public string EnterEvent;
        [Tooltip("Wwise event triggered on exit")]
        public string ExitEvent;

        public override void TriggerEnter()
        {
            
        }

        public override void TriggerExit()
        {

        }

        public override void Expose(GameObject go)
        {
        }

        public override void ShowGizmos()
        {
            OnDrawGizmosSelected();
        }

        void OnDrawGizmos()
        {
            if (KeepGizmo)
                OnDrawGizmosSelected();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
