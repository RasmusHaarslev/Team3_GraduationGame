using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

namespace Assets.ModuleDesigner.Scripts
{
    public class OnSceneStarter : MonoBehaviour
    {
        [Header("Gizmo options")]
        [Tooltip("Keep gizmo visible")]
        public bool KeepGizmo = true;
        public Mesh gizmoMesh;

        [Header("Trigger options")]
        public TriggerType TriggerOnOff;

        [Header("Output objects")]
        public TriggerReceiver[] Targets;

        public enum TriggerType
        {
            On, Off
        }

        void Start()
        {
            if (TriggerOnOff.ToString() == "On")
                TriggerEnter();
            else
                TriggerExit();
        }

        public void TriggerEnter()
        {
            foreach (var target in Targets)
            {
                target.TriggerEnter();
            }
        }

        public void TriggerExit()
        {
            foreach (var target in Targets)
            {
                target.TriggerExit();
            }
        }

        void OnDrawGizmos()
        {
            if (KeepGizmo)
                OnDrawGizmosSelected();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            //Gizmos.DrawCube(transform.position, transform.localScale);
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

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
