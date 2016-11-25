using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.ModuleDesigner.Scripts
{
    public class BoxTrigger : MonoBehaviour
    {
        [Header("Trigger options")]
        public TagEnum TagToTrigger;
        public Mesh gizmoMesh;

        [Header("Gizmo options")]
        [Tooltip("Keep gizmo visible")]
        public Boolean KeepGizmo = true;

        [Header("Output objects")]
        public List<TriggerReceiver> Targets;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == TagToTrigger.ToString())
            {
                foreach (var target in Targets)
                {
                    target.TriggerEnter();
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == TagToTrigger.ToString())
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
            Gizmos.color = Color.yellow;
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

        void OnValidate()
        {
            foreach (var target in Targets)
            {
                target.Expose(this.gameObject);
            }
        }
    }
}
