using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class PlayerInventory : MonoBehaviour
    {
        public UIManager uiManager;
        public PlayerManager playerManager;
        [HideInInspector]
        public WeaponSlotManager weaponSlotManager;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public WeaponItem unarmedWeapon;

        public List<WeaponItem> weaponsInventory; 
        public List<ConsumableItem> consumableInventory;
        public ConsumableItem[] consumableEquipmentSlot = new ConsumableItem[4];

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[4];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[4];

        public int currnetRightWeaponIndex = -1;
        public int currnetLeftWeaponIndex = -1;
        public int currnetConsumableIndex = 0;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            playerManager = GetComponent<PlayerManager>();
        }

        public void PickUpConsumableItem(ConsumableItem item)
        {
            consumableInventory.Add(item);
            uiManager.consumableInventoryUI.AddItem(this,item);
        }

        public void EquipmentWeapon(EquipmentSlotUI equipmentSlotUI, WeaponInventorySlot weaponInventorySlot)
        {
            WeaponItem temp = equipmentSlotUI.weapon;
            string equipmentName = equipmentSlotUI.name;
            int equipmentIndex = int.Parse(equipmentName.Substring(equipmentName.Length - 1)) - 1;
            if (equipmentName.StartsWith("Right"))
            {
                weaponsInRightHandSlots[equipmentIndex] = weaponInventorySlot.item;
                currnetRightWeaponIndex = equipmentIndex;
                rightWeapon = weaponInventorySlot.item;
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            }
            else
            {
                weaponsInLeftHandSlots[equipmentIndex] = weaponInventorySlot.item;
                currnetLeftWeaponIndex = equipmentIndex;
                leftWeapon = weaponInventorySlot.item;
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
            equipmentSlotUI.AddItem(weaponInventorySlot.item);
            weaponsInventory.Remove(weaponInventorySlot.item);

            if(temp != null)
                weaponsInventory.Add(temp);
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);

            consumableEquipmentSlot[0] = consumableInventory[0];
            currnetConsumableIndex = 0;
            consumableInventory.Remove(consumableInventory[0]);
            ChangeConsumableItem();

            uiManager.consumableInventoryUI.CreateUIItem(this);
        }

        public void ChangeRightWeapon()
        {
            WeaponItem weapon = null;
            while(weapon == null)
            {
                currnetRightWeaponIndex = currnetRightWeaponIndex >= weaponsInRightHandSlots.Length - 1 ? -1 : currnetRightWeaponIndex + 1;
                weapon = currnetRightWeaponIndex == -1 ? unarmedWeapon : weaponsInRightHandSlots[currnetRightWeaponIndex];
            }
            rightWeapon = weapon;
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        }

        public void ChangeLeftWeapon()
        {
            WeaponItem weapon = null;
            while (weapon == null)
            {
                currnetLeftWeaponIndex = currnetLeftWeaponIndex >= weaponsInLeftHandSlots.Length - 1 ? -1 : currnetLeftWeaponIndex + 1;
                weapon = currnetLeftWeaponIndex == -1 ? unarmedWeapon : weaponsInLeftHandSlots[currnetLeftWeaponIndex];
            }
            leftWeapon = weapon;
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public void ChangeConsumableItem()
        {
            int changeIndex = currnetConsumableIndex;
            ConsumableItem ci = null;
            while (ci == null)
            {
                currnetConsumableIndex = currnetConsumableIndex >= consumableEquipmentSlot.Length - 1 ? 0 : currnetConsumableIndex + 1;
                ci = consumableEquipmentSlot[currnetConsumableIndex];
                if (changeIndex == currnetConsumableIndex)
                {
                    break;
                }
            }
            weaponSlotManager.LoadConsumable(ci);
        }

        public void Tick()
        {
            HandleRelieveEquipment();
        }

        private void HandleRelieveEquipment()
        {
            if(playerManager.inputHandler.x_Input)
            {
                uiManager.PatchXInputEventToWindow();
            }
        }
    }
}
