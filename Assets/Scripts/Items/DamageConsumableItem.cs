using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    [CreateAssetMenu(menuName = "Items/Consumable Item/Damage Consumable Item")]
    public class DamageConsumableItem : ConsumableItem
    {
        PlayerManager playerManager;

        GameObject initThrowModel;

        [Header("Damage Setting")]
        public float baseDamage;
        public float boomRange;
        public float rangeDamage;
        public GameObject throwModel;
        public float forwardVelocity;
        public float upVelocity;
        public GameObject damageFX;
        public float damageFXClearTime;

        public override void UseConsumable(PlayerManager playerManager)
        {
            base.UseConsumable(playerManager);
            this.playerManager = playerManager;


        }

        public void HandleThrow()
        {
            //数量减少，并更新UI
            count--;
            playerManager.playerInventory.weaponSlotManager.LoadConsumable(this);

            Destroy(initUseModel);
            Destroy(initUseFX);

            initThrowModel = Instantiate(throwModel, playerManager.playerInventory.weaponSlotManager.rightHandSlot.consumablePivot.position, playerManager.cameraHandler.cameraPivotTransform.rotation);
            initThrowModel.transform.rotation = Quaternion.Euler(playerManager.cameraHandler.cameraPivotTransform.eulerAngles.x,playerManager.transform.eulerAngles.y,0);
            initThrowModel.GetComponent<ConsumableDamageCollider>().damageConsumableItem = this;
            Rigidbody rigidbody = initThrowModel.GetComponent<Rigidbody>();
            rigidbody.AddForce(initThrowModel.transform.forward * forwardVelocity);
            rigidbody.AddForce(initThrowModel.transform.up * upVelocity);
        }

        public void PlayDamageFX()
        {
            GameObject initDamageFX = Instantiate(damageFX,initThrowModel.transform.position,Quaternion.identity);
            ConsumableDamageCollider cdc = initDamageFX.GetComponent<ConsumableDamageCollider>();
            if (cdc != null)
            {
                initDamageFX.GetComponent<ConsumableDamageCollider>().damageConsumableItem = this;
            }
            Destroy(initDamageFX, damageFXClearTime);
        }
    }
}
