using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

namespace Assets.ModuleDesigner.Scripts
{
    public class Randomizer : TriggerReceiver
    {
        [Header("Randomizer options")]
        public Boolean IndividualRolls = true;
        [Range(0.0f, 100.0f)]
        public float ChanceForTrigger = 50.0f;

        [Header("Output objects")]
        public TriggerReceiver[] Targets;
        
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
            float collectiveRoll = Random.Range(0.0f, 100.0f);
            foreach (var target in Targets)
            {
                float individualRoll = Random.Range(0.0f, 100.0f);

                if ((collectiveRoll <= ChanceForTrigger && !IndividualRolls) || (individualRoll <= ChanceForTrigger && IndividualRolls))
                {
                    if (input)
                        target.TriggerEnter();
                    else
                        target.TriggerExit();
                }
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

        public override void Expose(GameObject go)
        {
        }

        public override void ShowGizmos()
        {
            OnDrawGizmos();
        }
    }
}
