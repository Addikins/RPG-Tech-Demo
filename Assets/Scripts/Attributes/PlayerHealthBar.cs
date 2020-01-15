using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] Image foregroundImage = null;
        [SerializeField] float updateSpeed = .2f;

        Health healthComponenet;

        private void Awake()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().OnHealthChange += HandleHealthChanged;
        }

        private void HandleHealthChanged(float difference)
        {
            StartCoroutine(AdjustHP(difference));
        }

        // Currently causes healthbar to sometimes show incorrectly

        private IEnumerator AdjustHP(float difference)
        {
            float preAdjustment = foregroundImage.fillAmount;
            float timeElapsed = 0f;

            while (timeElapsed < updateSpeed)
            {
                timeElapsed += Time.deltaTime;
                foregroundImage.fillAmount = Mathf.Lerp(preAdjustment, difference, timeElapsed / updateSpeed);
                yield return null;
            }
            foregroundImage.fillAmount = difference;
        }
    }
}