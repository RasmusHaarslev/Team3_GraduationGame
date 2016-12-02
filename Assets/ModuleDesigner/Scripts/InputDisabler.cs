using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class InputDisabler : TriggerReceiver
    {
        void Start()
        {
            
        }

        public override void TriggerEnter()
        {
            var player = Camera.main.GetComponent<CameraController>().player;
            player.GetComponent<MoveScript>().enabled = false;
            player.GetComponent<NavMeshAgent>().Stop();
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
            Gizmos.color = Color.green;
            //Gizmos.DrawSphere(transform.position + new Vector3(0, 0.75f, 0), 0.25f);
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

            //foreach (var obj in ObjectsToAffect)
            //{
            //    Gizmos.color = Color.green;
            //    Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), obj.transform.position- new Vector3(0, 0.5f, 0));
            //}
        }

    }
}
