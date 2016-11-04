﻿using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class CameraModifier : TriggerReceiver
    {
        [Header("Camera options")]
        [Tooltip("Moves the camera to the gizmo position")]
        public Boolean OverridePosition;
        [Space]
        [Tooltip("Check to override height")]
        public Boolean OverrideHeight;
        [Tooltip("The height of the camera")]
        public float Height;
        [Space]
        [Tooltip("Check to override distance")]
        public Boolean OverrideDistance;
        [Tooltip("The distance of the camera")]
        public float Distance;
        [Space]
        [Tooltip("Check to override slerp")]
        public Boolean OverrideSlerp;
        [Tooltip("The speed of camera movement")]
        public float Slerp;

        public override void TriggerEnter()
        {
            Camera.main.GetComponent<CameraController>().OverridePosition = OverridePosition;
            Camera.main.GetComponent<CameraController>().OverriddenPosition = this.transform.position;

            Camera.main.GetComponent<CameraController>().OverrideDistance = OverrideDistance;
            Camera.main.GetComponent<CameraController>().OverriddenDistance = Distance;

            Camera.main.GetComponent<CameraController>().OverrideHeight = OverrideHeight;
            Camera.main.GetComponent<CameraController>().OverriddenHeight = Height;

            Camera.main.GetComponent<CameraController>().OverrideSlerp = OverrideSlerp;
            Camera.main.GetComponent<CameraController>().OverriddenSlerp = Slerp;
        }

        public override void TriggerExit()
        {
            Camera.main.GetComponent<CameraController>().OverridePosition = false;
            Camera.main.GetComponent<CameraController>().OverrideDistance = false;
            Camera.main.GetComponent<CameraController>().OverrideHeight = false;
            Camera.main.GetComponent<CameraController>().OverrideSlerp = false;
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
            Gizmos.DrawSphere(transform.position, 0.25f);
            
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
