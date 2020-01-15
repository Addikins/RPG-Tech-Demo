using UnityEngine;

namespace RPG.Stats
{
    public class ExpBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;
        Experience experience;

        private void Start()
        {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }
        private void Update()
        {
            foreground.localScale = new Vector3(experience.GetComponent<BaseStats>().GetExpProgression(), 1, 1);
        }
    }
}