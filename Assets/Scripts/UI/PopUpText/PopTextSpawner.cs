using UnityEngine;

namespace RPG.UI.PopUpText
{
    public class PopTextSpawner : MonoBehaviour
    {
        [SerializeField] PopText popTextPrefab = null;

        public void Spawn(float amount)
        {
            PopText instance = Instantiate<PopText>(popTextPrefab, transform);
            instance.SetValue(amount);
        }
    }
}
