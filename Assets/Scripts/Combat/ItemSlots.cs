using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class ItemSlots : MonoBehaviour
    {
        [SerializeField] Toggle[] toggles;

        Fighter player;


        public void DisplayToggle(WeaponConfig weaponConfig)
        {
            foreach (Toggle toggle in toggles)
            {
                ItemSlot itemSlot = toggle.GetComponent<ItemSlot>();
                if (itemSlot.GetIsActive()) { continue; }

                if (itemSlot.GetItem() == weaponConfig)
                {
                    itemSlot.SetActiveStatus(true);
                    toggle.gameObject.SetActive(true);
                    Transform highlight = toggle.gameObject.transform.parent.GetChild(toggle.transform.GetSiblingIndex() - 1);
                    highlight.gameObject.SetActive(true);
                }
            }
        }

        public void EquipItem(WeaponConfig weaponConfig)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            player.EquipWeapon(weaponConfig);
        }

    }

}