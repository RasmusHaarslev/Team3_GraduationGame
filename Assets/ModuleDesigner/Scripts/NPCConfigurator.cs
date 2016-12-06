using Assets.ModuleDesigner.Scripts.BaseClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class NPCConfigurator : TriggerReceiver
    {
        [Header("Objects")]
        [Tooltip("Should contain objects the trigger should affect")]
        public List<GameObject> ObjectsToAffect;
        
        [Header("Configuration")]
        [Tooltip("Should contain objects the trigger should affect")]
        public bool ApplyOnStart;
        public int Health = 0;
        public int Damage = 0;
        public int Range = 0;
        public float DamageSpeed = 0;
        public float CurrentHealth = 0;

        void Start()
        {
            if (ApplyOnStart)
            {
                TriggerEnter();
            }
        }

        public override void TriggerEnter()
        {
            foreach (var obj in ObjectsToAffect)
            {
                obj.GetComponent<Character>().health = Health;
                obj.GetComponent<Character>().damage = Damage;
                obj.GetComponent<Character>().damageSpeed = DamageSpeed;
                obj.GetComponent<Character>().range = Range;
                obj.GetComponent<Character>().currentHealth = CurrentHealth;
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
