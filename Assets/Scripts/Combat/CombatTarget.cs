using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        PlayerController player;

        public bool HandleRaycast(PlayerController callingController)
        {
            player = callingController;
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
                // callingController.SetMoveIndicator(transform.position);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            if (player.GetComponent<Fighter>().GetWeapon().GetWeaponRange() > player.GetWeaponCursorRange())
            { return CursorType.Ranged; }
            return CursorType.Combat;
        }
    }
}