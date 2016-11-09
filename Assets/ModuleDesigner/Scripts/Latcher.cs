using System;
using System.Collections;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class Latcher : TriggerReceiver
    {
        [Header("Delay options")]
        public int DelayInSeconds = 0;

        [Header("Output objects")]
        public TriggerReceiver[] Targets;

        public override void TriggerEnter()
        {
            foreach (var target in Targets)
            {
                target.TriggerEnter();
            }
        }

        public override void TriggerExit()
        { 
            // Doesnt send off signal
        }

        void OnDrawGizmos()
        {
            if (KeepGizmo)
                OnDrawGizmosSelected();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

            foreach (var obj in Targets) 
            {
                obj.ShowGizmos();
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position + new Vector3(0, 0.5f, 0), obj.transform.position - new Vector3(0, 0.5f, 0));
            }
        }

        void OnValidate()
        {
            foreach (var target in Targets)
            {
                target.Expose(this.gameObject);
            }
        }

        public override void Expose(GameObject go)
        {

        }

        public override void ShowGizmos()
        {
            OnDrawGizmos();
        }
    }
}
