using RPG.Combat;
using UnityEngine;

namespace RPG.Cinematics
{
    public class CameraSwitch : MonoBehaviour
    {
        [SerializeField] float perspectiveCharacterOutline = 0.1f;
        [SerializeField] float orthographicCharacterOutline = 0.02f;

        Camera mainCamera;

        public void SwitchCameraMode()
        {

            mainCamera = Camera.main;
            if (mainCamera.orthographic == true)
            {
                mainCamera.orthographic = false;
                SetCharacterOutlines(perspectiveCharacterOutline);
                return;
            }
            SetCharacterOutlines(orthographicCharacterOutline);
            mainCamera.orthographic = true;
        }

        private void SetCharacterOutlines(float outlineThickness)
        {
            Fighter[] fighters = FindObjectsOfType<Fighter>();

            foreach (Fighter fighter in fighters)
            {
                fighter.SetOutlineThickness(outlineThickness);
            }
        }
    }
}