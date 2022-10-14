using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CF
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;
        public Image ConsumableIcon;
        public Text ConsumableCount;

        public void UpdateWeaponQuickSlotsUI(bool isLeft,WeaponItem weapon)
        {
            if(isLeft == false)
            {
                if (weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }else
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }

        internal void UpdateConsumableSlotUI(ConsumableItem currentConsumable)
        {
            if (currentConsumable != null)
            {
                ConsumableIcon.sprite = currentConsumable.itemIcon;
                ConsumableIcon.enabled = true;
                ConsumableCount.text = currentConsumable.count + "";
                ConsumableCount.enabled = true;
            }
            else
            {
                ConsumableIcon.sprite = null;
                ConsumableIcon.enabled = false;
                ConsumableCount.text = "";
                ConsumableCount.enabled = false;
            }
        }
    }
}
