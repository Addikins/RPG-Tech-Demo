using System;
using UnityEngine;
using TMPro;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyLevelDisplay : MonoBehaviour
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
                textMeshPro.text = "--";
                return;
            }
            Health health = fighter.GetTarget();
            textMeshPro.text = String.Format("{0}", health.GetCurrentLevel());
        }
    }
}