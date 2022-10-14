using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CF
{
    public class EnemyManager : CharacterManager
    {
        [Header("State Setting")]
        public int searchRadius;
        public LayerMask searchLayer;
        public int viewableAngle;
        public float pursueRangeMaxDistance;
        public float pursueRangeMinDistance;
        public float attackRange;
        public float minAttackInterval;
        public float maxAttackInterval;
        public CameraHandler cameraHandler;
        public Canvas enemyUI;

        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [HideInInspector]
        public EnemyEquipmentManager enemyEquipmentManager;
        [HideInInspector]
        public Rigidbody mRigidbody;
        [HideInInspector]
        public EnemyStats enemyStats;
        [HideInInspector]
        public EnemyStateMachine esm;
        

        protected override void Awake()
        {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();
            esm = GetComponentInChildren<EnemyStateMachine>();
            mRigidbody = GetComponent<Rigidbody>();
            enemyStats = GetComponent<EnemyStats>();
            enemyEquipmentManager = GetComponentInChildren<EnemyEquipmentManager>();
        }

        private void Start()
        {
            animatorManager.Initialize();
            enemyEquipmentManager.LoadWeapon();
        }

        //根据状态来行动
        protected override void Update()
        {
            base.Update();
            esm.Tick();

            animatorManager.anim.SetBool("IsAlive",enemyStats.IsAlive());
            //让enemyui跟着摄像机旋转
            RotateEnemyUIToCamera();
        }

        private void RotateEnemyUIToCamera()
        {
            enemyUI.transform.eulerAngles = new Vector3(cameraHandler.cameraPivotTransform.localEulerAngles.x, cameraHandler.cameraPivotTransform.localEulerAngles.y,0);
        }

        public void RestartGame()
        {
            transform.localPosition = new Vector3(-7.82f,0,0);
            transform.localRotation = Quaternion.identity;

            enemyStats.RestartGame();

            esm.RestartGame();
        }
    }
}
