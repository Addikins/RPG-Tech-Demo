using System;
using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;
        [SerializeField] float pickupRange = 2f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
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
                if (child.gameObject.tag != "Pickup Effect")
                {
                    child.gameObject.SetActive(shouldShow);
                }
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
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
            return CursorType.WeaponPickup;
        }
    }
}
