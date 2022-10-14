using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        bool isHitShield = false;
        CharacterStats characterStats;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider(CharacterStats stats)
        {
            damageCollider.enabled = true;
            characterStats = stats;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            isHitShield = false;
        }

        private void OnTriggerEnter(Collider collison)
        {
            if (collison.tag.EndsWith("Shield"))
            {
                isHitShield = true;
            }

            if (collison.tag.EndsWith("Player"))
            {
                if (isHitShield)
                {
                    isHitShield = false;
                    //EnemyStats enemyStats = characterStats as EnemyStats;
                    //enemyStats.animator.CrossFade("AttackInterrupt", 0.1f);
                }
                else
                {
                    PlayerStats playerStats = collison.GetComponent<PlayerStats>();
                    if (playerStats != null)
                    {
                        EnemyStats enemyStats = characterStats as EnemyStats;
                        if (playerStats.isParry)
                        {
                            enemyStats.attackPlayer = playerStats;
                            enemyStats.animator.CrossFade("AttackInterrupt", 0.1f);
                            playerStats.parryEnemy = enemyStats;
                        }
                        else
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(enemyStats.enemyManager.enemyEquipmentManager.rightWeapon.baseDamage));
                        }
                        playerStats.isParry = false;
                    }
                }
            }

            if (collison.tag.EndsWith("Enemy"))
            {
                if (isHitShield)
                {
                    //Debug.Log("播放打到盾上的动画");
                }
                else
                {
                    PlayerStats playerStats = characterStats as PlayerStats;
                    EnemyStats enemyStats = collison.GetComponent<EnemyStats>();
                    if (enemyStats != null)
                    {
                        enemyStats.TakeDamage(Mathf.RoundToInt(playerStats.playerManager.playerInventory.weaponSlotManager.attackingWeapon.baseDamage));
                    }
                    enemyStats.enemyManager.esm.pursueTarget = playerStats.playerManager;
                    enemyStats.enemyManager.esm.CheckState(enemyStats.enemyManager.esm.pursueState);
                }
            }

            if(collison.tag == "Illusionary")
            {
                collison.GetComponent<IllusionaryWall>().DisappaerWall();
            }
        }
    }
}
