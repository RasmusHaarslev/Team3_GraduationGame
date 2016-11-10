using System;
using System.Collections;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class LogicGate : TriggerReceiver
    {
        [Header("Logic Gate options")]
        public int InputToTrigger = 0;

        private int AmountOfInputs = 0;

        [Header("Output objects")]
        public TriggerReceiver[] Targets;

        public override void TriggerEnter()
        {
            AmountOfInputs++;

            if (AmountOfInputs >= InputToTrigger)
            {
                foreach (var target in Targets)
                {
                    target.TriggerEnter();
                }
            }
        }

        public override void TriggerExit()
        {
            AmountOfInputs--;

            if (AmountOfInputs < InputToTrigger)
            {
                foreach (var target in Targets)
                {
                    target.TriggerExit();
                }
            }
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

        public override void Expose(GameObject go)
        {

        }

        public override void ShowGizmos()
        {
            OnDrawGizmos();
        }
    }
}
