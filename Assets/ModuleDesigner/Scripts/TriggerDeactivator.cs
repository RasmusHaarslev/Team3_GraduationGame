using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class TriggerDeactivator : TriggerReceiver
    {
        public Mesh gizmoMesh;

        [Header("Objects")]
        [Tooltip("Should contain objects the trigger should affect")]
        public GameObject[] ObjectsToAffect;

        public override void TriggerEnter()
        {
            foreach (var obj in ObjectsToAffect)
            {
                obj.SetActive(false);
            }
        }

        public override void TriggerExit()
        {
            foreach (var obj in ObjectsToAffect)
            {
                obj.SetActive(true);
            }
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
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.color = ObjectsToAffect.Length > 0 ? Color.green : Color.red;
            //Gizmos.DrawSphere(transform.position + new Vector3(0, 0.75f, 0), 0.25f);
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

            foreach (var obj in ObjectsToAffect)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position + new Vector3(0,0.6f,0), obj.transform.position);
            }
        }
    }
}
