using System;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] Health health = null;
        TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            textMeshPro.text = String.Format("{0} / {1}", health.GetHealthPoints(), health.GetMaxHealth());
        }
    }
}