using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class ItemSlots : MonoBehaviour
    {
        [SerializeField] Toggle[] toggles;

        public void DisplayToggle(WeaponConfig weaponConfig)
        {
            print("Displaying Toggles");
            foreach (Toggle toggle in toggles)
            {
                if (toggle.GetComponent<ItemSlot>().GetItem() == weaponConfig)
                {
                    toggle.gameObject.SetActive(true);
                    Transform highlight = toggle.gameObject.transform.parent.GetChild(toggle.transform.GetSiblingIndex() - 1);
                    highlight.gameObject.SetActive(true);
                }
            }
        }
    }

}