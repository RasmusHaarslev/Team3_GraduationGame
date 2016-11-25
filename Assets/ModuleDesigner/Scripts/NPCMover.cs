using Assets.ModuleDesigner.Scripts.BaseClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class NPCMover : TriggerReceiver
    {
        [Header("Objects")]
        [Tooltip("Should contain objects the trigger should affect")]
        public List<GameObject> ObjectsToAffect;

        [Header("Target")]
        [Tooltip("Should contain the target where the NPC should navigate towards")]
        public Transform target;
         
        void Start()
        {
            
        }

        public override void TriggerEnter()
        {
            foreach (var obj in ObjectsToAffect)
            {
                obj.GetComponent<NavMeshAgent>().SetDestination(target.position);
            }
        }

        public override void TriggerExit()
        {
            
        }

        public override void Expose(GameObject go)
        {
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
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.color = ObjectsToAffect.Count > 0 ? Color.green : Color.red;
            //Gizmos.DrawSphere(transform.position + new Vector3(0, 0.75f, 0), 0.25f);
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

            #region checkingStuff
            List<GameObject> Removes = new List<GameObject>();
            foreach (var obj in ObjectsToAffect)
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
                    ObjectsToAffect.Remove(obj);
                }
            }
            #endregion

            foreach (var obj in ObjectsToAffect)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), obj.transform.position- new Vector3(0, 0.5f, 0));
            }
        }

    }
}
