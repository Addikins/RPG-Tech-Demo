using System;
using RPG.Stats;
using UnityEngine;
using TMPro;

namespace RPG.Combat
{
    public class EnemyLevelDisplay : MonoBehaviour
    {
        [SerializeField] BaseStats baseStats;
        TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            textMeshPro.text = String.Format("{0}", baseStats.GetLevel());
        }
    }
}