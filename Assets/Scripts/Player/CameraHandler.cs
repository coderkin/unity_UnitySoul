using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class CameraHandler : MonoBehaviour
    {
        public InputHandler inputHandle;
        public PlayerManager playerManager;

        public Transform lockOnTransform;
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private float cameraPivotTransfomPositionY;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.15f;
        public float pivotSpeed = 0.02f;
        float colliderYSpeed = 0;
        public float rotateSpeed = 3f;

        private float targetPosition;
        float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        public Transform leftLockTarget;
        public Transform rightLockTarget;
        public Transform currnetLockOnTarget;
        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public Transform nearestLockOnTarget;
        public float maximunLockOnDistance = 30;

        private void Awake()
        {
            singleton = this;
            playerManager = FindObjectOfType<PlayerManager>();
            inputHandle = FindObjectOfType<InputHandler>();
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = LayerMask.GetMask("Environment");
        }

        private void Start()
        {
            cameraPivotTransfomPositionY = cameraPivotTransform.position.y;
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position,targetTransform.position,ref cameraFollowVelocity,followSpeed);
            myTransform.position = targetPosition;
            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta,float mouseXInput,float mouseYInput)
        {
            if(inputHandle.lockOnFlag)
            {
                Vector3 dir = currnetLockOnTarget.position - lockOnTransform.position;
                dir.Normalize();
                cameraPivotTransform.localRotation = Quaternion.LookRotation(dir);
            }else
            {
#if UNITY_EDITOR
                lookAngle += mouseXInput * lookSpeed * delta;
                pivotAngle -= mouseYInput * pivotSpeed * delta;
#else
                lookAngle += mouseXInput * lookSpeed * delta / 2;
                pivotAngle -= mouseYInput * pivotSpeed * delta / 2;
#endif
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);
                cameraPivotTransform.localRotation = Quaternion.Euler(pivotAngle, lookAngle, 0);
            }
        }

        private void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if(Physics.SphereCast(cameraPivotTransform.position,cameraSphereRadius,direction,out hit,Mathf.Abs(targetPosition),ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);

                if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
                {
                    targetPosition = -minimumCollisionOffset;
                }

                cameraTransformPosition.z = Mathf.SmoothDamp(cameraTransform.localPosition.z, targetPosition, ref colliderYSpeed, followSpeed);
                cameraTransform.localPosition = cameraTransformPosition;
            }else
            {
                cameraTransformPosition.z = Mathf.SmoothDamp(cameraTransform.localPosition.z, defaultPosition, ref colliderYSpeed, followSpeed);
                cameraTransform.localPosition = cameraTransformPosition;
            }
        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position,26);
            for (int i = 0; i < colliders.Length; i++)
            {
                EnemyManager enemyManager = colliders[i].GetComponent<EnemyManager>();
                if(enemyManager != null)
                {
                    Vector3 lockTargetDirection = enemyManager.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position,enemyManager.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection,cameraTransform.forward);

                    if(enemyManager.transform.root != targetTransform.transform.root
                        && viewableAngle > -50 && viewableAngle < 50
                        && distanceFromTarget <= maximunLockOnDistance)
                    {
                        if (!Physics.Linecast(playerManager.lockOnTransform.position,enemyManager.lockOnTransform.position,LayerMask.GetMask("Environment")))
                        {
                            availableTargets.Add(enemyManager);
                        }
                    }
                }
            }

            for (int i = 0; i < availableTargets.Count; i++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[i].transform.position);
                if(distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[i].lockOnTransform;
                }
            }

            if (nearestLockOnTarget != null)
            {
                currnetLockOnTarget = nearestLockOnTarget;
                inputHandle.lockOnFlag = true;
                cameraPivotTransform.localPosition = new Vector3(0, cameraPivotTransfomPositionY, 0);
            }
            else
            {
                inputHandle.lockOnFlag = false;
            }
        }

        public void FindLeftAndRightEnemy()
        {
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;
            if (inputHandle.lockOnFlag)
            {
                for (int i = 0; i < availableTargets.Count; i++)
                {
                    Vector3 relativeEnemyPosition = currnetLockOnTarget.InverseTransformPoint(availableTargets[i].transform.position);
                    float distanceFromLeftTarget = currnetLockOnTarget.transform.position.x - availableTargets[i].transform.position.x;
                    float distanceFromRightTarget = currnetLockOnTarget.transform.position.x + availableTargets[i].transform.position.x;
                    if(relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[i].transform;
                    }

                    if(relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[i].transform;
                    }

                }
            }
        }

        public void HandleLockOnChange(bool isLeft)
        {
            if(inputHandle.lockOnFlag)
            {
                if(isLeft && leftLockTarget != null)
                {
                    currnetLockOnTarget = leftLockTarget;
                }else if(!isLeft && rightLockTarget != null)
                {
                    currnetLockOnTarget = rightLockTarget;
                }
            }
        }

        public void ClearLockOnTargets()
        {
            cameraPivotTransform.localPosition = new Vector3(0, cameraPivotTransfomPositionY, 0);

            //重置x轴的旋转角度
            lookAngle = cameraPivotTransform.localEulerAngles.y;

            inputHandle.lockOnFlag = false;
            availableTargets.Clear();
            currnetLockOnTarget = null;
            nearestLockOnTarget = null;
        }

        public void RestartGame()
        {
            transform.position = Vector3.zero;

            lookAngle = 0;
            pivotAngle = 0;
            cameraPivotTransform.localPosition = new Vector3(0,2,0);
            cameraPivotTransform.localRotation = Quaternion.identity;

            cameraTransform.localPosition = new Vector3(0,0,-6);
            cameraTransform.localRotation = Quaternion.identity;

            currnetLockOnTarget = null;
            inputHandle.lockOnFlag = false;
        }
    }
}
