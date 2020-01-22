using UnityEngine;

namespace RPG.Cinematics
{
    public class TriggerVFX : MonoBehaviour
    {
        [SerializeField] GameObject VFXToTrigger;
        [SerializeField] Transform triggerLocation;
        [SerializeField] bool destroyOnTrigger = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Instantiate(VFXToTrigger, triggerLocation, true);
                gameObject.SetActive(!destroyOnTrigger);
            }
        }
    }

}