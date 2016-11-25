using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace Assets.ModuleDesigner.Scripts
{
    public class OnSceneStarter : MonoBehaviour
    {
        [Header("Gizmo options")]
        [Tooltip("Keep gizmo visible")]
        public bool KeepGizmo = true;
        public Mesh gizmoMesh;

        [Header("Trigger options")]
        public TriggerType Output;

        [Header("Output objects")]
        public List<TriggerReceiver> Targets;

        public enum TriggerType
        {
            On, Off
        }

        void Start()
        {
			// TODO: fix this shit
            if (Output.ToString() == "On")
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

            #region checkingStuff
            List<TriggerReceiver> Removes = new List<TriggerReceiver>();
            foreach (var obj in Targets)
            {
                if (obj == null)
                {
                    Removes.Add(obj);
                }
            }
            foreach (var obj in Removes)
            {
                if (obj == null)
                {
                    Targets.Remove(obj);
                }
            }
            #endregion

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
