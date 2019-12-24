using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] float fadeOutTime = 3f;
        [SerializeField] float fadeInTime = 1f;

        CanvasGroup canvasGroup;

        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();

            StartCoroutine(FadeOutIn());
        }

        IEnumerator FadeOutIn()
        {
            yield return FadeOut(fadeOutTime);
            print("Faded out");
            yield return FadeIn(fadeInTime);
            print("Faded in");
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}