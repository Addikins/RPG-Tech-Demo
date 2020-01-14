using UnityEngine;

namespace RPG.Stats
{
    public class ExpBar : MonoBehaviour
    {
        [SerializeField] Experience experience = null;
        [SerializeField] RectTransform foreground = null;

        private void Update()
        {
            foreground.localScale = new Vector3(experience.GetComponent<BaseStats>().GetExpProgression(), 1, 1);
        }
    }
}