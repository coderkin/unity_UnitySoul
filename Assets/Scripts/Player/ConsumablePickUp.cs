using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CF
{
    public class ConsumablePickUp : Interactable
    {
        public ConsumableItem item;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            //播放动画
            playerManager.animatorManager.PlayTargetAnimation("pick_up_item", true);
            //显示提示
            playerManager.itemPopUIGameObject.GetComponentInChildren<Text>().text = item.itemName;
            playerManager.itemPopUIGameObject.GetComponentInChildren<RawImage>().texture = item.itemIcon.texture;
            playerManager.itemPopUIGameObject.SetActive(true);
            //加到库存
            playerManager.playerInventory.PickUpConsumableItem(item);
            //更新UI
            
            //销毁pickup
            Destroy(gameObject);
        }
    }
}
