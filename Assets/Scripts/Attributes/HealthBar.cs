using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponenet = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;
        [SerializeField] float disableDelayTime = 3f;
        [SerializeField] bool isPlayer = false;

        float timeDead = 0;

        private void Update()
        {
            if (!isPlayer)
            {
                if (healthComponenet.IsDead())
                {
                    timeDead += Time.deltaTime;
                    if (timeDead >= disableDelayTime)
                    {
                        rootCanvas.enabled = false;
                        return;
                    }
                }

                if (Mathf.Approximately(healthComponenet.GetHealthFraction(), 1))
                {
                    rootCanvas.enabled = false;
                    return;
                }
                rootCanvas.enabled = true;
            }

            foreground.localScale = new Vector3(healthComponenet.GetHealthFraction(), 1, 1);

        }
    }
}