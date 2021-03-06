using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using RPG.SceneManagement;
using GameDevTV.Utils;
using UnityEngine.Events;
using System.Collections;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] GainHealthEvent gainHealth;
        [SerializeField] GainExperienceEvent gainEXP;
        [SerializeField] UnityEvent onDie;
        [SerializeField] int deathAnimations = 2;
        [SerializeField] float onDeathLoadDelay = 3f;
        [SerializeField] GameObject takeDamageVFX = null;
        [SerializeField] GameObject deathVFX = null;
        [SerializeField] GameObject healVFX = null;

        public event Action<float> OnHealthChange = delegate { };

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        [System.Serializable]
        public class GainHealthEvent : UnityEvent<float> { }

        [System.Serializable]
        public class GainExperienceEvent : UnityEvent<float> { }

        LazyValue<float> healthPoints;

        bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min((healthPoints.value + healthToRestore), GetInitialHealth());
            gainHealth.Invoke(healthToRestore);
            Instantiate(healVFX, transform.position + Vector3.up, Quaternion.identity);

        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            takeDamage.Invoke(damage);

            if (gameObject.CompareTag("Player"))
            {
                OnHealthChange(GetHealthFraction());
            }

            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                TriggerDeath();
                AwardExperience(instigator);
                return;
            }
            Instantiate(takeDamageVFX, transform.position + Vector3.up, transform.rotation);
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetCurrentLevel()
        {
            return GetComponent<BaseStats>().GetLevel();
        }

        public float GetAttackDamage()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Damage);
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetHealthPercentage()
        {
            return GetHealthFraction() * 100;
        }

        public float GetHealthFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void TriggerDeath()
        {
            if (isDead) { return; }

            isDead = true;
            int randomAnimation = UnityEngine.Random.Range(0, deathAnimations);
            GetComponent<Animator>().SetTrigger("die" + randomAnimation);
            GetComponent<ActionScheduler>().CancelCurrentAction();

            
            // GetComponent<CapsuleCollider>().enabled = false;

            Instantiate(deathVFX, transform.position + Vector3.up, Quaternion.identity);

            if (gameObject.CompareTag("Player"))
            {
                SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
                // StartCoroutine(savingWrapper.LoadLastScene());
                StartCoroutine(ReloadSave());
            }
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
            OnHealthChange(GetHealthFraction());
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }
            if (instigator.GetComponent<Health>().IsDead()) { return; }

            float xpToGain = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);

            experience.GainExperience(xpToGain);
            gainEXP.Invoke(Mathf.Round(xpToGain));
        }

        public IEnumerator ReloadSave()
        {
            yield return new WaitForSeconds(onDeathLoadDelay);
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Load();
            StartCoroutine(savingWrapper.FadeInOnLoad());
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            OnHealthChange(GetHealthFraction());

            if (healthPoints.value <= 0)
            {
                TriggerDeath();
            }
            else if (isDead == true)
            {
                ResetDeathTriggers();

                isDead = false;
            }
        }

        private void ResetDeathTriggers()
        {
            for (int i = 0; i < deathAnimations; i++)
            {
                GetComponent<Animator>().ResetTrigger("die" + i);
            }
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("reload");
        }
    }

}