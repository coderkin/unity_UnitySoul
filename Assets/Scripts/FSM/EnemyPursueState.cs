using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class EnemyPursueState : BaseState
    {
        /*
         *  敌人AI：
            默认idle状态，当player进入追击范围，开始追击，当进入攻击范围时，开始攻击，当player逃离追击范围，返回idle

            状态：
            idle  -> pursue
            pursue -> attack   pursue -> idle
            attack -> pursue
            idle 
         */
        float distance;
        public override void HandleState(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            if(!esm.pursueTarget.playerStates.IsAlive())
            {
                esm.CheckState(esm.idleState);
                return;
            }

            if (enemyManager.isInteracting)
            {
                return;
            }

            if (esm.pursueState == null)
            {
                esm.CheckState(esm.idleState);
                return;
            }

            distance = Vector3.Distance(enemyManager.transform.position, esm.pursueTarget.transform.position);
            if (distance > enemyManager.pursueRangeMaxDistance)
            {
                esm.CheckState(esm.idleState);
                return;
            }

            if (distance > enemyManager.pursueRangeMinDistance)
            {
                enemyManager.animatorManager.UpdateAnimatorValues(1, 0, false);
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(esm.pursueTarget.transform.position);
                RotateToTarget(esm,enemyManager);
            }
            else
            {
                esm.CheckState(esm.attackState);
            }
        }
    }
}
