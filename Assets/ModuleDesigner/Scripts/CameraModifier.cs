using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class CameraModifier : TriggerReceiver
    {
        [Header("Camera options")]
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
        [Tooltip("The amount of angle added to the camera rotation compared to the player"), Range(-10.0f,10.0f)]
        public float XRotationOffset;

        public override void TriggerEnter()
        {
            print("Enter!");
            Camera.main.GetComponent<CameraController>().OverridePosition = OverridePosition;
            Camera.main.GetComponent<CameraController>().OverriddenPosition = this.transform.position;

            Camera.main.GetComponent<CameraController>().OverrideDistance = OverrideDistance;
            Camera.main.GetComponent<CameraController>().OverriddenDistance = Distance;

            Camera.main.GetComponent<CameraController>().XRotationOffset = XRotationOffset;

            Camera.main.GetComponent<CameraController>().OverrideHeight = OverrideHeight;
            Camera.main.GetComponent<CameraController>().OverriddenHeight = Height;

            Camera.main.GetComponent<CameraController>().OverrideSlerp = OverrideSlerp;
            Camera.main.GetComponent<CameraController>().OverriddenSlerp = Slerp;

            Camera.main.GetComponent<CameraController>().OverrideRotationSlerp = OverrideRotationSlerp;
            Camera.main.GetComponent<CameraController>().OverriddenRotationSlerp = RotationSlerp;

            Camera.main.GetComponent<CameraController>().SlerpBack = false;
        }

        public override void TriggerExit()
        {
            Camera.main.GetComponent<CameraController>().OverridePosition = false;
            Camera.main.GetComponent<CameraController>().OverrideDistance = false;
            Camera.main.GetComponent<CameraController>().OverrideHeight = false;
            Camera.main.GetComponent<CameraController>().OverrideSlerp = false;
            Camera.main.GetComponent<CameraController>().OverrideRotationSlerp = false;
            Camera.main.GetComponent<CameraController>().XRotationOffset = 0f;
            Camera.main.GetComponent<CameraController>().SlerpBack = SlerpBack;
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
        }

    }
}
