using UnityEngine;
using System;
using TMPro;


namespace RPG.Combat
{
    public class EquippedWeaponDisplay : MonoBehaviour
    {
        Fighter fighter;
        TextMeshProUGUI textMeshPro;

        private void Start()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            textMeshPro.text = String.Format("{0}", fighter.GetWeapon().name);
        }
    }
}