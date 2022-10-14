using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class EnemyAttackState : BaseState
    {
        /*
         *  敌人AI：
            默认idle状态，当player进入追击范围，开始追击，当进入攻击范围时，开始攻击，当player逃离追击范围，返回idle

            判断是否是连击状态，是则直接连击，否则：
                判断是否在攻击距离内，
                在攻击距离内，判断是否在攻击间隔中，不在随机数攻击（连击判断），在间隔中则举盾静止/前后游离
                不在攻击距离内，追击/举盾静止/前后游离

         */
        string[] comboAnims;
        bool isCombo;
        int comboIndex = 0;
        float distance;
        float randomInterval;
        float randomVal = 0;
        float attackInterval;
        float horizontalMovement = -1;
        float vertivalMovement;
        bool isStatic;
        bool isDynamic;

        public override void HandleState(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            //player death
            if (!esm.pursueTarget.playerStates.IsAlive() || !enemyManager.enemyStats.IsAlive())
            {
                esm.CheckState(esm.idleState);
                return;
            }

            if(enemyManager.enemyStats.getParried)
            {
                StopMove(enemyManager);
                horizontalMovement = -1;
                return;
            }

            //enemy 正在(攻击中，被攻击中)
            if (enemyManager.isInteracting)
            {
                RotateToTarget(esm, enemyManager);
                LiftShield(enemyManager,false);
                StopMove(enemyManager);
                return;
            }

            if (isCombo)    //combo
            {
                HandleCombo(esm, enemyManager);
                return;
            }

            if(attackInterval > 0) //攻击间隔
            {
                attackInterval -= Time.deltaTime;
                HandleAttackInterval(esm,enemyManager);
                return;
            }

            distance = Vector3.Distance(enemyManager.transform.position, esm.pursueTarget.transform.position);
            if (distance > enemyManager.attackRange)//不在攻击范围
            {
                esm.CheckState(esm.pursueState);
                return;
            }

            HandleAttack(esm, enemyManager);
        }

        private void HandleAttack(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            enemyManager.animatorManager.PlayTargetAnimation(enemyManager.enemyEquipmentManager.rightWeapon.oh_light_attack_1, true);
            attackInterval = Random.Range(enemyManager.minAttackInterval,enemyManager.maxAttackInterval);
            randomInterval = 0;
            //判断连击
            if (RandomResult(0, 2, 1))
            {
                isCombo = true;
                comboIndex = 0;
            }
        }

        private void HandleAttackInterval(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            if (randomInterval > 0)
            {
                randomInterval -= Time.deltaTime;
                HandleStroll(esm, enemyManager);
                return;
            }
            randomInterval = 2;
            randomVal = Random.Range(0, 3);//0 静止,  12 游离
            if(randomVal == 0)
            {
                HandleStatic(esm,enemyManager);
            }else
            {
                HandleDynamic(esm, enemyManager);
            }
        }

        private void HandleStatic(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            StopMove(enemyManager);
            horizontalMovement = -1;
            LiftShield(enemyManager, true);
        }

        private void HandleDynamic(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            randomVal = Random.Range(0, 5);//01向左走,  2后aw退， 34向右走

            if(randomVal < 2)
            {
                LiftShield(enemyManager, false);
                horizontalMovement = -0.75f;
                vertivalMovement = 0;
            }
            else if(randomVal == 2)
            {
                LiftShield(enemyManager, true);
                horizontalMovement = 0;
                vertivalMovement = -1;
            }
            else
            {
                LiftShield(enemyManager, false);
                horizontalMovement = 0.75f;
                vertivalMovement = 0;
            }
        }

        private void HandleStroll(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            if (horizontalMovement != -1)
            {
                Vector3 direction = enemyManager.transform.right * horizontalMovement + enemyManager.transform.forward * (vertivalMovement == -1 ? -0.1f : 0);
                direction.Normalize();
                enemyManager.mRigidbody.velocity = direction;

                enemyManager.animatorManager.anim.SetFloat("Vertical", vertivalMovement);
                enemyManager.animatorManager.anim.SetFloat("Horizontal", horizontalMovement);
            }
        }

        private bool RandomResult(int min, int max, int val)
        {
            if (Random.Range(min, max) < val)
            {
                return true;
            }
            return false;
        }

        private void HandleCombo(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            if(comboAnims == null)
            {
                comboAnims = new string[] { enemyManager.enemyEquipmentManager.rightWeapon.oh_light_attack_2, enemyManager.enemyEquipmentManager.rightWeapon.oh_light_attack_3 };
            }
            enemyManager.animatorManager.PlayTargetAnimation(comboAnims[comboIndex], true);

            if(RandomResult(0,3,1))
            {
                comboIndex++;
                if(comboIndex >= comboAnims.Length)
                {
                    isCombo = false;
                }
            }else
            {
                isCombo = false;
            }
        }

        private void StopMove(EnemyManager enemyManager)
        {
            enemyManager.animatorManager.UpdateAnimatorValues(0, 0, false);
            enemyManager.navMeshAgent.enabled = false;
            enemyManager.mRigidbody.velocity = Vector3.zero;
        }

        private void LiftShield(EnemyManager enemyManager,bool isLiftShield)
        {
            enemyManager.animatorManager.anim.SetBool("IsShield", isLiftShield);
            if (enemyManager.isInteracting)
            {
                enemyManager.enemyEquipmentManager.SetLeftShieldColliderEnbale(false);
            }
            else
            {
                enemyManager.enemyEquipmentManager.SetLeftShieldColliderEnbale(isLiftShield);
            }
        }
    }
}
