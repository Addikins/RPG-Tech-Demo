using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] WeaponConfig itemForThisSlot = null;

        public WeaponConfig GetItem()
        {
            return itemForThisSlot;
        }
    }

}