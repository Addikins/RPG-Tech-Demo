using System;
using RPG.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyDamageDisplay : MonoBehaviour
    {
        [SerializeField] Health health;
        TextMeshProUGUI textMeshPro;


        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            textMeshPro.text = String.Format("{0}", health.GetAttackDamage());
        }
    }
}