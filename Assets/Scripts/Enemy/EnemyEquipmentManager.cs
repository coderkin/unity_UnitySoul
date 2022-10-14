using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF {
    public class EnemyEquipmentManager : MonoBehaviour
    {
        public WeaponItem leftWeapon;
        public WeaponItem rightWeapon;

        public WeaponHolderSlot leftWeaponHolderSlot;
        public WeaponHolderSlot rightWeaponHolderSlot;

        ShieldCollider leftHandShieldCollider;
        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;
        public EnemyManager enemyManager;


        public void LoadWeapon()
        {
            if(leftWeapon != null)
            {
                GameObject leftModel = Instantiate(leftWeapon.modelPrefab,leftWeaponHolderSlot.parentOverride.transform);
                leftHandShieldCollider = leftModel.GetComponentInChildren<ShieldCollider>();
                leftHandDamageCollider = leftModel.GetComponentInChildren<DamageCollider>();

            }

            if (rightWeapon != null)
            {
                GameObject rigthModel = Instantiate(rightWeapon.modelPrefab, rightWeaponHolderSlot.transform);
                rightHandDamageCollider = rigthModel.GetComponentInChildren<DamageCollider>();
            }
        }

        public void SetLeftShieldColliderEnbale(bool enable)
        {
            leftHandShieldCollider.SetShieldColliderEnable(enable);
        }

        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider(enemyManager.enemyStats);
        }

        public void CloseRightDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void DrainStaminaLightAttack()
        {
            
        }
    }
}
