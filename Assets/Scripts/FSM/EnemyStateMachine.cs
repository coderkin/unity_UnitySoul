using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF {
    public class EnemyStateMachine : MonoBehaviour
    {
        [Header("States")]
        public EnemyIdleState idleState;
        public EnemyPursueState pursueState;
        public EnemyAttackState attackState;

        private EnemyManager enemyManager;
        public BaseState defaultState;

        public BaseState currentState;

        [HideInInspector]
        public PlayerManager pursueTarget;

        private void Awake()
        {
            enemyManager = GetComponentInParent<EnemyManager>();
            currentState = defaultState;
        }

        public void Tick()
        {
            currentState.HandleState(this,enemyManager);
        }

        public void CheckState(BaseState checkState)
        {
            currentState = checkState;
        }

        public void RestartGame()
        {
            pursueTarget = null;
            currentState = defaultState;
        }
    }
}
