using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CF
{
    public class EquipmentWindowUI : UIWinodow
    {
        //public WeaponItem weapon;

        EquipmentSlotUI[] es;
        public UIManager uiManager;

        private void Awake()
        {
            es = GetComponentsInChildren<EquipmentSlotUI>();
        }

        public void OnButtonClick(EquipmentSlotUI equipmentSlotUI)
        {
            uiManager.CurrentIntactbaleWindow(this);
        }

        public void LoadWeaponOnEquipmentScreen(PlayerInventory playerInventory)
        {
            if (es == null)
            {
                es = GetComponentsInChildren<EquipmentSlotUI>();
            }
            for (int i = 0; i < playerInventory.weaponsInRightHandSlots.Length; i++)
            {
                for (int j = 0; j < es.Length; j++)
                {
                    if(es[j].name.StartsWith("Right") && es[j].name.EndsWith(Convert.ToString(i + 1)))
                    {
                        es[j].AddItem(playerInventory.weaponsInRightHandSlots[i]);
                    }
                }
            }

            for (int i = 0; i < playerInventory.weaponsInLeftHandSlots.Length; i++)
            {
                for (int j = 0; j < es.Length; j++)
                {
                    if (es[j].name.StartsWith("Left") && es[j].name.EndsWith(Convert.ToString(i + 1)))
                    {
                        es[j].AddItem(playerInventory.weaponsInLeftHandSlots[i]);
                    }
                }
            }
        }

        public override void HandleXInput(PlayerInventory playerInventory)
        {
            
        }
    }
}
