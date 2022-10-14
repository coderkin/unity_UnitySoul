using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class WeaponSlotManager : MonoBehaviour
    {
        QuickSlotsUI quickSlotsUI;

        WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;

        ShieldCollider leftHandShieldCollider;
        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        public ConsumableItem currentConsumable;

        Animator animtor;

        PlayerStats playerStats;

        public WeaponItem attackingWeapon;

        private void Awake()
        {
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            animtor = GetComponent<Animator>();
            WeaponHolderSlot[] weaponHodlerSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponHolderSlot in weaponHodlerSlots)
            {
                if (weaponHolderSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponHolderSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true,weaponItem);
                LoadLeftWeaponDamageCollider();
                #region Handle Left Weapon Idle Animation
                if(weaponItem != null) {
                    animtor.CrossFade(weaponItem.left_hand_idle,0.2f);
                }else
                {
                    animtor.CrossFade("left_arm_idle", 0.2f);
                }
                #endregion
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                LoadRightWeaponDamageCollider();
                #region Handle Left Weapon Idle Animation
                if (weaponItem != null)
                {
                    animtor.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }
                else
                {
                    animtor.CrossFade("right_arm_idle", 0.2f);
                }
                #endregion
            }
        }

        public void LoadConsumable(ConsumableItem item)
        {
            currentConsumable = item;
            quickSlotsUI.UpdateConsumableSlotUI(currentConsumable);
        }

        #region Handle Weapons Damage Collider

        public void LoadLeftWeaponDamageCollider()
        {
            leftHandShieldCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<ShieldCollider>();
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void SetLeftShieldColliderEnbale(bool enable)
        {
            leftHandShieldCollider.SetShieldColliderEnable(enable);
        }

        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider(playerStats);
        }

        public void CloseLeftDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }

        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider(playerStats);
        }

        public void CloseRightDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        #endregion

        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMutiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMutiplier));
        }
    }
}
