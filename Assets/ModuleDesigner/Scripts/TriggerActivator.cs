using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class TriggerActivator : TriggerReceiver
    {
        public override void Trigger()
        {
            foreach (var obj in ObjectsToAffect)
            {
                obj.SetActive(true);
            }
        }

        void OnDrawGizmos()
        {
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + new Vector3(0, 0.5f, 0), 0.5f);
            Gizmos.color = ObjectsToAffect.Length > 0 ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + new Vector3(0, 1.5f, 0), 0.25f);

            foreach (var obj in ObjectsToAffect)
            {
                Gizmos.DrawLine(this.transform.position, obj.transform.position);
            }
        }

    }
}
