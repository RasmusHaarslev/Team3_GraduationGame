using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class SphereTrigger : MonoBehaviour
    {
        public Mesh gizmoMesh;

        [Header("Trigger options")]
        public TagEnum TagToTrigger;

        [Header("Gizmo options")]
        [Tooltip("Keep gizmo visible")]
        public bool KeepGizmo = true;

        [Header("Output objects")]
        public TriggerReceiver[] Targets;

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
            if (other.tag == "Player")
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
            Gizmos.DrawWireSphere(transform.position, transform.localScale.x/2);
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

            foreach (var obj in Targets)
            {
                obj.ShowGizmos();
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, obj.transform.position);
            }
        }

        void OnValidate()
        {
            /*foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags)
            {
                print(tag);
            }*/

            foreach (var target in Targets)
            {
                target.Expose(this.gameObject);
            }
        }
    }
}
