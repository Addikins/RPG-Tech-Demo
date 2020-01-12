using UnityEngine;

namespace RPG.UI.PopUpText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] PopText popTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            PopText instance = Instantiate<PopText>(damageTextPrefab, transform);
            instance.SetValue(damageAmount);
        }
    }
}
