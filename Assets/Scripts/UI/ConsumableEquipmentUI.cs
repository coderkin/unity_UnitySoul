using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CF
{
    public class ConsumableEquipmentUI : UIWinodow
    {
        public UIManager uiManger;
        public Image[] consumableImages;
        public ConsumableItem changeItem;
        private int clickIndex;

        bool isInit;

        private void Awake()
        {
            if (!isInit)
            {
                init();
            }
        }

        private void init()
        {
            isInit = true;
            consumableImages = new Image[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                int index = i;
                consumableImages[i] = transform.GetChild(i).Find("Slot Background/Item Icon").GetComponent<Image>();
                transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { OnGridItemClick(index); });
            }
        }

        private void OnGridItemClick(int index)
        {
            uiManger.CurrentIntactbaleWindow(this);
            clickIndex = index;
        }

        public void UpdateUI(PlayerInventory playerInventory)
        {
            if (!isInit)
            {
                init();
            }
            ConsumableItem[] consumables = playerInventory.consumableEquipmentSlot;
            for (int i = 0; i < consumables.Length; i++)
            {
                if(consumables[i] != null)
                {
                    consumableImages[i].sprite = consumables[i].itemIcon;
                    consumableImages[i].enabled = true;
                }else
                {
                    consumableImages[i].sprite = null;
                    consumableImages[i].enabled = false;
                }
            }
        }

        public void EquipmentConsumableItem(PlayerInventory playerInventory, int changeIndex)
        {
            changeItem = playerInventory.consumableEquipmentSlot[clickIndex];
            playerInventory.consumableEquipmentSlot[clickIndex] = playerInventory.consumableInventory[changeIndex];
            if (changeItem != null)
            {
                playerInventory.consumableInventory[changeIndex] = changeItem;
                uiManger.consumableInventoryUI.ChangeItem(playerInventory, changeItem ,changeIndex);
            }
            else
            {
                playerInventory.consumableInventory.RemoveAt(changeIndex);
                uiManger.consumableInventoryUI.DeleteItem(playerInventory, changeIndex);
            }
            UpdateUI(playerInventory);

            //更新QuickSlotUI
            if(clickIndex == 0)
            {
                playerInventory.weaponSlotManager.LoadConsumable(playerInventory.consumableEquipmentSlot[clickIndex]);
            }
        }

        public override void HandleXInput(PlayerInventory playerInventory)
        {
            ConsumableItem temp = playerInventory.consumableEquipmentSlot[clickIndex];
            //取消装备栏,更新UI
            playerInventory.consumableEquipmentSlot[clickIndex] = null;
            consumableImages[clickIndex].sprite = null;
            consumableImages[clickIndex].enabled = false;


            //重新添加回playerInventory
            playerInventory.consumableInventory.Add(temp);
            uiManger.consumableInventoryUI.AddItem(playerInventory,temp);

            playerInventory.ChangeConsumableItem();
        }
    }
}
