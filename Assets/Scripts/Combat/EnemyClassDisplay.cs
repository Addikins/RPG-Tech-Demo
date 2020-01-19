using System;
using UnityEngine;
using TMPro;
using RPG.Stats;

namespace RPG.Combat
{
    public class EnemyClassDisplay : MonoBehaviour
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
                textMeshPro.text = "--------";
                return;
            }
            BaseStats baseStats = fighter.GetTarget().GetComponent<BaseStats>();
            textMeshPro.text = String.Format("{0}", baseStats.GetCharacterClass());
        }
    }
}