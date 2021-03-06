using System;
using RPG.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyDamageDisplay : MonoBehaviour
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
                textMeshPro.text = "---";
                return;
            }
            Health health = fighter.GetTarget();
            textMeshPro.text = String.Format("{0}", Mathf.Round(health.GetAttackDamage()));
        }
    }
}