using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public abstract class BaseState : MonoBehaviour
    {
        public abstract void HandleState(EnemyStateMachine esm,EnemyManager enemyManager);

        protected void RotateToTarget(EnemyStateMachine esm, EnemyManager enemyManager)
        {
            Vector3 rt = esm.pursueTarget.transform.position - enemyManager.transform.position;
            rt.y = 0;
            rt.Normalize();

            Quaternion forward = Quaternion.LookRotation(rt);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, forward, 0.05f);
        }
    }
}