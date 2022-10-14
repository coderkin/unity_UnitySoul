using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class WeaponInventoryWindowUI : MonoBehaviour
    {
        public UIManager uiManager;
        public EquipmentSlotUI equipmentSlotUI;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void OnEquipmentSlotClick(EquipmentSlotUI equipmentSlotUI)
        {
            this.equipmentSlotUI = equipmentSlotUI;
        }

        public void OnWeaponInventorySlotClick(WeaponInventorySlot weaponInventorySlot)
        {
            uiManager.playerInventory.EquipmentWeapon(equipmentSlotUI, weaponInventorySlot);
            uiManager.UpdateUI();
        }
    }
}
