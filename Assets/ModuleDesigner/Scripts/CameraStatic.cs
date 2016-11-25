using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class CameraStatic : TriggerReceiver
    {
        public float SlerpAmount;

        private Camera mainCamera;
        private Camera targetCamera;
        private bool Enabled = false;

        void Start()
        {
            mainCamera = Camera.main;
            targetCamera = GetComponentInChildren<Camera>();
        }

        void Update()
        {
            if (Enabled)
            {
                mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, targetCamera.transform.position, Time.deltaTime * SlerpAmount);
                mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetCamera.transform.rotation, Time.deltaTime * SlerpAmount);
            }
        }

        public override void TriggerEnter()
        {
            mainCamera.GetComponent<CameraController>().enabled = false;
            Enabled = true;
        }

        public override void TriggerExit()
        {
            mainCamera.GetComponent<CameraController>().enabled = true;
            Enabled = false;
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
