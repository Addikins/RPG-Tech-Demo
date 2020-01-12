using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Stats
{
    public class DamageDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            textMeshPro.text = String.Format("{0}", baseStats.GetStat(Stat.Damage));
        }
    }
}