using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            textMeshPro.text = String.Format("{0} / {1}", Mathf.Round(health.GetHealthPoints()), health.GetMaxHealth());
        }
    }
}