using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class PlayerAttacker : MonoBehaviour
    {

        PlayerAnimatorManager animatorHandler;
        InputHandler inputHandler;
        public string lastAttack;
        WeaponSlotManager weaponSlotManager;
        PlayerManager playerManager;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if(inputHandler.comboFlag)
            {
                animatorHandler.DisableCombo();
                if (lastAttack == weapon.oh_light_attack_1)
                {
                    lastAttack = weapon.oh_light_attack_2;
                    animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_2,true);
                }else if (lastAttack == weapon.oh_light_attack_2)
                {
                    lastAttack = weapon.oh_light_attack_3;
                    animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_3, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_1, !weapon.isUnarmed);
            lastAttack = weapon.oh_light_attack_1;
        }

        public void HandleParryAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation("Parry_Attack", true);
            playerManager.playerStates.parryEnemy.enemyManager.animatorManager.PlayTargetAnimation("Getting_Parried", true);
            playerManager.playerStates.parryEnemy.TakeDamageNoAnim(Mathf.RoundToInt(weaponSlotManager.attackingWeapon.baseDamage * weaponSlotManager.attackingWeapon.parryMutiplier));
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.oh_heavy_attack_1, !weapon.isUnarmed);
            lastAttack = weapon.oh_heavy_attack_1;
        }

        public void HandleConsumable()
        {
            if (playerManager.playerInventory.weaponSlotManager.currentConsumable != null)
            {
                playerManager.playerInventory.weaponSlotManager.currentConsumable.UseConsumable(playerManager);
            }
        }

        public void HandleParry()
        {
            if (!playerManager.isInteracting)
            {
                playerManager.isInteracting = true;
                animatorHandler.PlayTargetAnimation("Parry", true);
            }
        }
    }
}
