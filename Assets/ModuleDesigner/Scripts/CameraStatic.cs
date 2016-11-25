using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class CameraStatic : TriggerReceiver
    {
        private Camera mainCamera;
        private Camera switchedCamera;

        void Start()
        {
            mainCamera = Camera.main;
            switchedCamera = GetComponentInChildren<Camera> ();
        }

        public override void TriggerEnter()
        {
            mainCamera.enabled = false;
            switchedCamera.enabled = true;
        }

        public override void TriggerExit()
        {
            mainCamera.enabled = true;
            switchedCamera.enabled = false;
        }

        public override void Expose(GameObject go)
        {
            OnValidate();
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
            Gizmos.color = new Color(1,0,0,0.5f);
            //Gizmos.DrawSphere(transform.position, 0.25f);
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

            var CameraPos = this.GetComponentInChildren<Camera>().gameObject.transform.position;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(this.transform.position + new Vector3(0, 0.5f, 0), CameraPos - new Vector3(0, 0.5f, 0));
        }

        void OnValidate()
        {
            
        }

    }
}
