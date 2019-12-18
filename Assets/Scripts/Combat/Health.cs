using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            // if (health - damage >= 0)
            // {
            //     health -= damage;
            // }
            // else
            // {
            //     health = 0;
            // }
        }
    }

}