using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class CameraSwitch : TriggerReceiver
    {
        private Camera mainCamera;
        private Camera switchedCamera;

        void Start()
        {
            mainCamera = Camera.current;
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
            this.GetComponentInChildren<CameraHelper>().Target = go;
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
        }

        void OnValidate()
        {
            
        }

    }
}
