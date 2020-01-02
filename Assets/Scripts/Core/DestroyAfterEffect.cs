using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;
        [SerializeField] bool destroyOnSpawn = false;

        DestroyAfterEffect[] otherEffects;

        private void Start()
        {
            DestroyOtherEffects();
        }

        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (targetToDestroy != null)
                {
                    Destroy(targetToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void DestroyOtherEffects()
        {
            otherEffects = FindObjectsOfType<DestroyAfterEffect>();
            foreach (DestroyAfterEffect otherEffect in otherEffects)
            {
                if (otherEffect == this) { continue; }
                if (otherEffect != null && otherEffect.destroyOnSpawn)
                {
                    print("Destroying Effect");
                    Destroy(otherEffect.gameObject);
                }
            }
        }
    }

}