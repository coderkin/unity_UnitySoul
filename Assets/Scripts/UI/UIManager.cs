using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class UIManager : MonoBehaviour
    {
        public UIWinodow currentWindow;

        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;
        public GameObject consumableInventoryWindowUI;
        [HideInInspector]
        public ConsumableInventoryUI consumableInventoryUI;
        public ConsumableEquipmentUI consumableEquipmentUI;

        [Header("UI Window")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;
        public GameObject gameOverWindow;
        public GameObject leftPanel;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotParent;
        WeaponInventorySlot[] weaponInventorySlots;

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
            consumableInventoryUI = consumableInventoryWindowUI.GetComponentInChildren<ConsumableInventoryUI>();
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if(i < playerInventory.weaponsInventory.Count)
                {
                    if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab,weaponInventorySlotParent);
                        weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
            equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
            consumableEquipmentUI.UpdateUI(playerInventory);
        }

        public void OpenOrCloseSelectWindow(bool openOrClose)
        {
            selectWindow.SetActive(openOrClose);
        }

        public void CloseAllInventoryWindow()
        {
            weaponInventoryWindow.SetActive(false);
            equipmentWindowUI.gameObject.SetActive(false);
            consumableInventoryWindowUI.gameObject.SetActive(false);
            leftPanel.SetActive(false);
        }

        public void HandleConsumableItemEquipment(PlayerInventory playerInventory,int changeIndex)
        {
            consumableEquipmentUI.EquipmentConsumableItem(playerInventory, changeIndex);
        }

        public void CurrentIntactbaleWindow(UIWinodow winodow)
        {
            currentWindow = winodow;
        }

        public void PatchXInputEventToWindow()
        {
            if (currentWindow != null)
            {
                currentWindow.HandleXInput(playerInventory);
            }
        }
    }
}
