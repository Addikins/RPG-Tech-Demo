using UnityEngine;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class PortalEntrance : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // callingController.SetMoveIndicator(transform.position);
                callingController.InteractWithMovement();
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Portal;
        }
    }
}