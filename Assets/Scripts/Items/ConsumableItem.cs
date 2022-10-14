using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class ConsumableItem : Item
    {
        public enum ConsumableType
        {
            Damage,
            Heal
        }

        public ConsumableType type;
        public int count;
        public GameObject useModel;
        public GameObject useFX;
        public string useAnimation;

        protected GameObject initUseModel;
        protected GameObject initUseFX;

        public virtual void UseConsumable(PlayerManager playerManager)
        {
            if(count > 0)
            {
                playerManager.animatorManager.PlayTargetAnimation(useAnimation,true);

                initUseModel = Instantiate(useModel, playerManager.playerInventory.weaponSlotManager.rightHandSlot.consumablePivot);
                if(useFX != null)
                {
                    initUseFX = Instantiate(useFX, initUseModel.transform);
                }

                playerManager.animatorEventHandler.consumableItem = this;
            }else
            {
                //无法使用
                playerManager.animatorManager.PlayTargetAnimation("CantUse", true);
            }
        }
    }
}
