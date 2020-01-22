using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float chaseSpeedMultiplier = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] GameObject shaderSource = null;
        [SerializeField] Color targetOutlineColor = Color.red;
        [SerializeField] Color defaultOutlineColor = Color.black;
        [Range(0, 1)]
        [SerializeField] float defaultOutlineThickness = .02f;

        Health target;
        Health lastKnownTarget;
        float timeSinceLastAttack = Mathf.Infinity;

        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            SetOutlineThickness(defaultOutlineThickness);
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead())
            {
                ResetAnimationTriggers();
                return;
            }

            if (!GetIsInRange(target.transform))
            {
                GetComponent<Mover>().MoveTo(target.transform.position, chaseSpeedMultiplier);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);

            if (gameObject.tag == "Player")
            {
                ItemSlots itemSlots = FindObjectOfType<ItemSlots>();
                itemSlots.DisplayToggle(currentWeaponConfig);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public WeaponConfig GetWeapon()
        {
            return currentWeaponConfig;
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // Triggers Hit() Event
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            int randomAnimation = UnityEngine.Random.Range(0, currentWeaponConfig.GetAnimationOverrides());
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack" + randomAnimation);
        }

        private void ResetAnimationTriggers()
        {
            for (int i = 0; i < currentWeaponConfig.GetAnimationOverrides(); i++)
            {
                GetComponent<Animator>().ResetTrigger("attack" + i);
            }
        }

        private void StopAttack()
        {
            ResetAnimationTriggers();
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        // Animation Event
        void Hit()
        {
            if (target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetWeaponRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }

            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform))
            {
                return false;
            }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            Health currentTarget = combatTarget.GetComponent<Health>();
            CancelPreviousAttack(currentTarget);

            GetComponent<ActionScheduler>().StartAction(this);
            target = currentTarget;

            SetTargetOutlineColor();
        }

        public void SetOutlineThickness(float outlineThickness)
        {
            if (shaderSource == null) { return; }
            shaderSource.GetComponent<Renderer>().material.SetFloat("_Outline", outlineThickness);
        }

        private void CancelPreviousAttack(Health currentTarget)
        {
            // Save target property in lastKnownTarget before reassigning target to the new target value
            if (target != currentTarget)
            {
                lastKnownTarget = target;
                target = null;
                SetTargetOutlineColor();
            }
        }

        private void SetTargetOutlineColor()
        {
            if (gameObject.tag != "Player") { return; }

            // Failsafe, may or may not be necessary
            if (target == null && lastKnownTarget == null) { return; }

            // If target is null then player is either moving or targeting a new enemy
            // So we take the last known target and reset their target outline
            if (target == null)
            {
                // Get target's shader component
                GameObject targetShaderSource = lastKnownTarget.GetComponent<Fighter>().shaderSource;
                // Change target's outline color
                targetShaderSource.GetComponent<Renderer>().material.SetColor("_OutlineColor", defaultOutlineColor);
            }
            // Sets the target's outline to the color set in the editor
            else
            {
                GameObject targetShaderSource = target.GetComponent<Fighter>().shaderSource;
                targetShaderSource.GetComponent<Renderer>().material.SetColor("_OutlineColor", targetOutlineColor);
            }
        }


        public void Cancel()
        {
            StopAttack();
            lastKnownTarget = target;

            // Assign target to lastTarget to avoid null reference exception
            target = null;

            SetTargetOutlineColor();

            GetComponent<Mover>().Cancel();
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
            target = null;
        }
    }
}
