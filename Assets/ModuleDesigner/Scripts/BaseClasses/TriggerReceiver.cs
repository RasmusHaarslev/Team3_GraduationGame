using UnityEngine;

namespace Assets.ModuleDesigner.Scripts.BaseClasses
{
    public abstract class TriggerReceiver : MonoBehaviour
    {
        [Tooltip("Should contain objects the activator should disable")]
        public GameObject[] ObjectsToAffect;

        // override this
        public abstract void Trigger();
    }
}
