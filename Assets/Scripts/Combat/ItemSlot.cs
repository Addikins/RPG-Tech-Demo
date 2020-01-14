using UnityEngine;

namespace RPG.Combat
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] WeaponConfig itemForThisSlot = null;
        [SerializeField] bool isActive = false;

        public WeaponConfig GetItem()
        {
            return itemForThisSlot;
        }

        public bool GetIsActive()
        {
            return isActive;
        }

        public void SetActiveStatus(bool activeStatus)
        {
            isActive = activeStatus;
        }
    }

}