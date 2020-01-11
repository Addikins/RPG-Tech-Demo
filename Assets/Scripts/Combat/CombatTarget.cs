using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        [SerializeField] GameObject shaderSource = null;
        [SerializeField] Color targetOutlineColor = Color.magenta;

        

        private void Start() {
            targetOutlineColor.a = .5f;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
                shaderSource.GetComponent<Renderer>().material.SetColor("_OutlineColor", targetOutlineColor);
                callingController.SetMoveIndicator(transform.position);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}