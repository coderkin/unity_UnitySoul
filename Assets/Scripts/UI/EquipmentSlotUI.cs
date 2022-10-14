using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CF
{
    public class EquipmentSlotUI : MonoBehaviour
    {
        public Image itemIcon;

        public WeaponItem weapon;

        private void Awake()
        {
            itemIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        }

        public void AddItem(WeaponItem newWeapon)
        {
            if(newWeapon == null)
            {
                return;
            }
            if(itemIcon == null)
            {
                itemIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            }
            itemIcon.sprite = newWeapon.itemIcon;
            itemIcon.enabled = true;
            weapon = newWeapon;
        }

        public void ClearItem()
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            weapon = null;
        }
    }
}
