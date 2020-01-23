using System;
using System.Collections;
using RPG.Attributes;
using RPG.Control;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;
        [SerializeField] float pickupRange = 2f;
        [SerializeField] bool disableOnPickup = false;
        [SerializeField] float attackBonus = 0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            if (attackBonus > 0)
            {
                subject.GetComponent<Fighter>().GetWeapon().AddWeaponDamage(attackBonus);
            }
            if (disableOnPickup)
            {
                gameObject.SetActive(false);
                return;
            }
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                if (!child.gameObject.CompareTag("Pickup Effect"))
                {
                    child.gameObject.SetActive(shouldShow);
                }
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // callingController.SetMoveIndicator(transform.position);

                if (IsInRange(callingController))
                {
                    Pickup(callingController.gameObject);
                    return true;
                }
                callingController.InteractWithMovement();
            }
            return true;
        }

        private bool IsInRange(PlayerController callingController)
        {
            return Vector3.Distance(transform.position, callingController.transform.position) < pickupRange;
        }

        public CursorType GetCursorType()
        {
            if (healthToRestore > 0)
            {
                return CursorType.HealthPot;
            }
            return CursorType.WeaponPickup;
        }
    }
}
