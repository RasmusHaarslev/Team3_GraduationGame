using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace Assets.ModuleDesigner.Scripts
{
    public class PseudoRandomizer : TriggerReceiver
    {
        [Header("Randomizer options")]
        public int AmountOfTriggers = 0;

        [Header("Output objects")]
        public List<TriggerReceiver> Targets;

        void Start()
        {
            Randomize(true);
        }

        public override void TriggerEnter()
        {
            Randomize(true);
        }

        public override void TriggerExit()
        {
            Randomize(false);
        }

        void Randomize(Boolean input)
        {
            foreach (var target in Targets)
            {
                if (AmountOfTriggers == 0)
                    break;

                if (Random.Range(0, 2) == 0 && AmountOfTriggers > 0)
                {
                    AmountOfTriggers--;

                    if (input)
                        target.TriggerEnter();
                    else
                        target.TriggerExit();
                }
            }

            if(AmountOfTriggers > 0)
                Randomize(input);
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
                Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), obj.transform.position - new Vector3(0, 0.5f, 0));
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
