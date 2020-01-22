using RPG.Attributes;
using UnityEngine;
using TMPro;
using System;

namespace RPG.Combat
{
    public class TutorialSignage : MonoBehaviour
    {
        [SerializeField] Health tutorialEnemy = null;
        [SerializeField] WeaponPickup weaponPickup = null;
        [SerializeField] TextMeshProUGUI textMeshPro = null;
        [SerializeField] string newText = null;

        private void Update()
        {
            ChangeOnDeath();
            ChangeOnPickup();
        }

        private void ChangeOnPickup()
        {
            if (weaponPickup == null) { return; }
            if (weaponPickup.gameObject.active) { return; }
            textMeshPro.text = newText;
            GetComponent<TutorialSignage>().enabled = false;
        }

        private void ChangeOnDeath()
        {
            if (tutorialEnemy == null) { return; }
            if (tutorialEnemy.IsDead())
            {
                textMeshPro.text = newText;
                GetComponent<TutorialSignage>().enabled = false;
            }
        }
    }
}