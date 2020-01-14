using System;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            textMeshPro.text = String.Format("{0} / {1}",
                Mathf.Round(experience.GetExperiencePoints()),
                Mathf.Round(experience.GetComponent<BaseStats>().ExpToNextLevelUp()));
        }
    }
}