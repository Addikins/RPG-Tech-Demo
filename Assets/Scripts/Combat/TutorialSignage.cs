using RPG.Attributes;
using UnityEngine;
using TMPro;

namespace RPG.Combat
{
    public class TutorialSignage : MonoBehaviour
    {
        [SerializeField] Health tutorialEnemy = null;
        [SerializeField] TextMeshProUGUI textMeshPro = null;
        [SerializeField] string newText = null;

        private void Update()
        {
            ChangeOnDeath();
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