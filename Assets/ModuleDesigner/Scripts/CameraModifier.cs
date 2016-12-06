using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class CameraModifier : TriggerReceiver
    {
        [Header("Camera options")]
        [Tooltip("Lock the cameras movement")]
        public bool LockCameraPosition;
        [Tooltip("Moves the camera to the gizmo position")]
        public bool OverridePosition;
        [Space]
        [Tooltip("Check to override height")]
        public bool OverrideHeight;
        [Tooltip("The height of the camera")]
        public float Height;
        [Space]
        [Tooltip("Check to override distance")]
        public bool OverrideDistance;
        [Tooltip("The distance of the camera")]
        public float Distance;
        [Space]
        [Tooltip("Check to override slerp")]
        public bool OverrideSlerp;
        [Tooltip("The speed of camera movement")]
        public float Slerp;

        [Tooltip("Check to override slerp")]
        public bool OverrideRotationSlerp;
        [Tooltip("The speed of camera movement")]
        public float RotationSlerp;
        [Tooltip("Check to override slerp")]
        public bool SlerpBack;

        [Space]
        [Tooltip("Check to override slerp")]
        public bool OverrideFogHeight;
        public float OverriddenFogHeight;
        [Tooltip("Check to override slerp")]
        public bool OverrideFogHeightDensity;
        public float OverriddenFogHeightDensity;
        [Tooltip("Check to override slerp")]
        public bool OverrideFogStartDistance;
        public float OverriddenFogStartDistance;

        private CameraController cameraController;

        [Space]
        [Tooltip("The amount of angle added to the camera rotation compared to the player"), Range(-10.0f,10.0f)]
        public float XRotationOffset;

        public override void TriggerEnter()
        {
            cameraController = Camera.main.GetComponent<CameraController>();

            cameraController.LockCameraPosition = LockCameraPosition;

            cameraController.OverridePosition = OverridePosition;
            cameraController.OverriddenPosition = this.gameObject.transform.position;

            cameraController.OverrideDistance = OverrideDistance;
            cameraController.OverriddenDistance = Distance;

            cameraController.XRotationOffset = XRotationOffset;

            cameraController.OverrideHeight = OverrideHeight;
            cameraController.OverriddenHeight = Height;

            cameraController.OverrideSlerp = OverrideSlerp;
            cameraController.OverriddenSlerp = Slerp;

            cameraController.OverrideRotationSlerp = OverrideRotationSlerp;
            cameraController.OverriddenRotationSlerp = RotationSlerp;

            cameraController.SlerpBack = false;

            cameraController.OverriddenFogHeight = OverriddenFogHeight;
            cameraController.OverrideFogHeight = OverrideFogHeight;

            cameraController.OverriddenFogHeightDensity = OverriddenFogHeightDensity;
            cameraController.OverrideFogHeightDensity = OverrideFogHeightDensity;

            cameraController.OverrideFogStartDistance = OverrideFogStartDistance;
            cameraController.OverriddenFogStartDistance = OverriddenFogStartDistance;
        }

        public override void TriggerExit()
        {
            cameraController = Camera.main.GetComponent<CameraController>();
            cameraController.OverridePosition = false;
            cameraController.OverrideDistance = false;
            cameraController.OverrideHeight = false;
            cameraController.OverrideSlerp = false;
            cameraController.OverrideRotationSlerp = false;
            cameraController.OverrideFogHeight = false;
            cameraController.OverrideFogHeightDensity = false;
            cameraController.OverrideFogStartDistance = false;
            cameraController.XRotationOffset = 0f;
            cameraController.SlerpBack = SlerpBack;
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
            CameraHelper helper = this.GetComponentInChildren<CameraHelper>();
            helper.OverridePosition = OverridePosition;
            helper.OverriddenPosition = this.transform.position;

            helper.OverrideDistance = OverrideDistance;
            helper.OverriddenDistance = Distance;

            helper.OverrideHeight = OverrideHeight;
            helper.OverriddenHeight = Height;

            helper.Update();

            TriggerEnter();
        }

    }
}
