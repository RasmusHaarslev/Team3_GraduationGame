using System;
using System.Collections;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.ModuleDesigner.Scripts
{
    public class Inverter : TriggerReceiver
    {
        [Header("Output objects")]
        public List<TriggerReceiver> Targets;

        public override void TriggerEnter()
        {
            foreach (var target in Targets)
            {
                target.TriggerExit(); 
            }
        }

        public override void TriggerExit()
        {
            foreach (var target in Targets)
            {
                target.TriggerEnter();
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

            #region checkingStuff
            List<TriggerReceiver> Removes = new List<TriggerReceiver>();
            foreach (var obj in Targets)
            {
                if (obj == null)
                {
                    Removes.Add(obj);
                }
            }
            foreach (var obj in Removes)
            {
                if (obj == null)
                {
                    Targets.Remove(obj);
                }
            }
            #endregion


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
