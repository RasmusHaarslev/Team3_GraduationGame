using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class CameraTransition : TriggerReceiver
    {
        [Tooltip("Slerp from and to normal gameplay camera")]
        public float SlerpAmount;
        [Tooltip("Amount of seconds to pan between cameras")]
        public float transitionTime;

        private float transitionSpeed;
        private float timelinePos = 0f;
        private Camera mainCamera;
        private Camera startCamera;
        private Camera endCamera;
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private bool Enabled = false;
        private float originalFOV;

        void Start()
        {
            transitionSpeed = 1f / transitionTime;
            mainCamera = Camera.main;
            originalFOV = mainCamera.fieldOfView;
            startCamera = GetComponentsInChildren<Camera>()[0];
            endCamera = GetComponentsInChildren<Camera>()[1];
        }

        void Update()
        {
            if (Enabled)
            {
                timelinePos += Time.deltaTime * transitionSpeed;
                targetPosition = Vector3.Slerp(startCamera.transform.position, endCamera.transform.position, timelinePos);
                targetRotation = Quaternion.Slerp(startCamera.transform.rotation, endCamera.transform.rotation, timelinePos);
                var targetFOV = Mathf.Lerp(startCamera.fieldOfView, endCamera.fieldOfView, timelinePos);

                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * SlerpAmount);
                mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, targetPosition, Time.deltaTime * SlerpAmount);
                mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, Time.deltaTime * SlerpAmount);
            }
            else
            {
                if (Mathf.Abs(mainCamera.fieldOfView - originalFOV) > 0.05f)
                {
                    mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originalFOV, Time.deltaTime * SlerpAmount);
                }
            }
        }

        public override void TriggerEnter()
        {
            transitionSpeed = 1f / transitionTime;
            mainCamera.GetComponent<CameraController>().enabled = false;
            Enabled = true;
        }

        public override void TriggerExit()
        {
            mainCamera.GetComponent<CameraController>().enabled = true;
            Enabled = false;
            timelinePos = 0;
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

            var CameraPos = this.GetComponentsInChildren<Camera>()[0].gameObject.transform.position;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(this.transform.position + new Vector3(0, 0.5f, 0), CameraPos - new Vector3(0, 0.5f, 0));

            var CameraPos2 = this.GetComponentsInChildren<Camera>()[1].gameObject.transform.position;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(this.transform.position + new Vector3(0, 0.5f, 0), CameraPos2 - new Vector3(0, 0.5f, 0));
        }

        void OnValidate()
        {
            
        }

    }
}
