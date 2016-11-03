using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class TriggerModule : MonoBehaviour
    {
        public ITriggerReceiver[] Targets;

        void OnCollisionEnter(Collider col)
        {
            foreach (var target in Targets)
            {
                target.Trigger();
            }
        }
    }
}
