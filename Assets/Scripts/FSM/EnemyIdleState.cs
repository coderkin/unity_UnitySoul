using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
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
    public class EnemyIdleState : BaseState
    {
        Collider[] results;
        float angle;
        public override void HandleState(EnemyStateMachine esm,EnemyManager enemyManager)
        {
            if(!enemyManager.enemyStats.IsAlive())
            {
                return;
            }

            enemyManager.animatorManager.UpdateAnimatorValues(0, 0, false);
            enemyManager.navMeshAgent.enabled = false;
            
            results = Physics.OverlapSphere(enemyManager.transform.position,enemyManager.searchRadius,enemyManager.searchLayer);
            foreach (var result in results)
            {
                //需要判断角度
                PlayerManager playerManager = result.GetComponent<CharacterManager>() as PlayerManager;
                if(!playerManager.playerStates.IsAlive())
                {
                    return;
                }

                Vector3 targetPosition = playerManager.transform.position - enemyManager.transform.position;

                angle = Vector3.Angle(enemyManager.transform.forward, targetPosition);
                //float signedAngle = Vector3.SignedAngle(enemyManager.transform.forward, targetPosition, Vector3.up);
                //Debug.Log(angle + "    -----    " + signedAngle);

                //超过可视角度
                if(angle < enemyManager.viewableAngle)
                {
                    esm.pursueTarget = playerManager;
                    esm.CheckState(esm.pursueState);
                }
            }
        }
    }
}
