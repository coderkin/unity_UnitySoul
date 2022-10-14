using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CF
{
    public class ConsumableInventoryUI : MonoBehaviour
    {
        public GameObject consumablePrefab;
        public List<ConsumableItem> consumableItems = new List<ConsumableItem>();
        public UIManager uiManger;

        private bool isInit = false;

        //创建item
        public void CreateUIItem(PlayerInventory playerInventory)
        {
            if(isInit)
            {
                return;
            }
            isInit = true;
            
            consumableItems.AddRange(playerInventory.consumableInventory);

            //从playerInventory更新消耗品的数量
            for (int i = 0; i < consumableItems.Count; i++)
            {
                Transform consumableInventorySlot = Instantiate(consumablePrefab, transform).transform;
                Image image = consumableInventorySlot.Find("Background/Item Icon").GetComponent<Image>();
                image.sprite = consumableItems[i].itemIcon;
                image.enabled = true;
            }

            //设置点击事件
            for (int i = 0; i < consumableItems.Count; i++)
            {
                SetGridClick(playerInventory, i);
            }
        }

        public void AddItem(PlayerInventory playerInventory,ConsumableItem item)
        {
            consumableItems.Add(item);
            Transform consumableInventorySlot = Instantiate(consumablePrefab, transform).transform;
            Image image = consumableInventorySlot.Find("Background/Item Icon").GetComponent<Image>();
            image.sprite = item.itemIcon;
            image.enabled = true;
            SetGridClick(playerInventory, consumableItems.Count - 1);
        }

        public void ChangeItem(PlayerInventory playerInventory, ConsumableItem item, int index)
        {
            consumableItems[index] = item;
            Transform consumableInventorySlot = transform.GetChild(index).transform;
            Image image = consumableInventorySlot.Find("Background/Item Icon").GetComponent<Image>();
            image.sprite = item.itemIcon;
            image.enabled = true;
        }

        public void DeleteItem(PlayerInventory playerInventory, int item)
        {
            //for (int i = 0; i < consumableItems.Count; i++)
            //{
            //    if(consumableItems[i] == item)
            //    {
                    
            //        break;
            //    }
            //}
            consumableItems.RemoveAt(item);
            Destroy(transform.GetChild(item).gameObject);

            for (int i = 0; i < consumableItems.Count; i++)
            {
                SetGridClick(playerInventory, i);
            }
        }

        private void SetGridClick(PlayerInventory playerInventory,int index)
        {
            Button btn = transform.GetChild(index).GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnGridItemClick(playerInventory,index));
        }

        private void OnGridItemClick(PlayerInventory playerInventory,int index)
        {
            if (playerInventory.consumableInventory[index] != null)
            {
                //如果item不为空，就赋值到装备栏上去
                uiManger.HandleConsumableItemEquipment(playerInventory,index);
            }
        }
    }
}
