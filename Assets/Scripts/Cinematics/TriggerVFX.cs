using UnityEngine;

namespace RPG.Cinematics
{
    public class TriggerVFX : MonoBehaviour
    {
        [SerializeField] GameObject VFXToTrigger;
        [SerializeField] Transform triggerLocation;
        private bool alreadyTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !alreadyTriggered)
            {
                Instantiate(VFXToTrigger, triggerLocation);
                alreadyTriggered = true;
            }
        }
    }

}