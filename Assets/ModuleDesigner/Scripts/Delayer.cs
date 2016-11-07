using System;
using System.Collections;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using UnityEditor;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

namespace Assets.ModuleDesigner.Scripts
{
    public class Delayer : TriggerReceiver
    {
        [Header("Delay options")]
        public int DelayInSeconds = 0;

        [Header("Output objects")]
        public TriggerReceiver[] Targets;

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
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, transform.localScale);

            foreach (var obj in Targets)
            {
                obj.ShowGizmos();
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, obj.transform.position);
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
