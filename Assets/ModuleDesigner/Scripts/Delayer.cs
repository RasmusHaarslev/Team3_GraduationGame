using System;
using System.Collections;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.ModuleDesigner.Scripts
{
    public class Delayer : TriggerReceiver
    {
        [Header("Delay options")]
        public float DelayInSeconds = 0f;

        [Header("Output objects")]
        public List<TriggerReceiver> Targets;

        public override void TriggerEnter()
        {
            StartCoroutine(CallForward(true));
        }

        public override void TriggerExit()
        {
            StartCoroutine(CallForward(false));
        }

        IEnumerator CallForward(Boolean input)
        {
            yield return new WaitForSeconds(DelayInSeconds);

            foreach (var target in Targets)
            {
                if (input)
                    target.TriggerEnter();
                else
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
            Gizmos.color = Color.blue;
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

        public override void Expose(GameObject go)
        {

        }

        public override void ShowGizmos()
        {
            OnDrawGizmos();
        }
    }
}
