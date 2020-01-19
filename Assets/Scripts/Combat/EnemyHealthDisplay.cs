using System;
using RPG.Attributes;
using UnityEngine;
using TMPro;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                textMeshPro.text = "--- / ---";
                return;
            }
            Health health = fighter.GetTarget();
            textMeshPro.text = String.Format("{0} / {1}", Mathf.Round(health.GetHealthPoints()), Mathf.Round(health.GetMaxHealth()));
        }
    }
}