using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            TriggerDeath();
            // if (health - damage >= 0)
            // {
            //     health -= damage;
            // }
            // else
            // {
            //     health = 0;
            // }
        }

        private void TriggerDeath()
        {
            if (!isDead && healthPoints == 0)
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }
    }

}