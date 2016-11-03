using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class TriggerModule : MonoBehaviour
    {
        public TriggerReceiver[] Targets;

        void OnTriggerEnter(Collider other)
            {
            foreach (var target in Targets)
            {
                target.Trigger();
            }
        }
    }
}
