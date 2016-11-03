using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class TriggerActivator : MonoBehaviour, ITriggerReceiver
    {
        [Tooltip("Should contain objects the activator should enable")]
        public GameObject[] ObjectsToActivate;

        public void Trigger()
        {
            foreach (var obj in ObjectsToActivate)
            {
                obj.SetActive(true);
            }
        }

    }
}
