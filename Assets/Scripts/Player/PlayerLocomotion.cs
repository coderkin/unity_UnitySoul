using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;
        PlayerManager playerManager;

        public bool isRoll;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimatorManager animatorHandler;

        public new Rigidbody rigidbody;

        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField] float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement State")]
        [SerializeField] float walkingSpeed = 1f;
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] float sprintSpeed = 7.5f;
        [SerializeField] float rotationSpeed = 10;
        [SerializeField] float fallingSpeed = 45;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            playerManager = GetComponent<PlayerManager>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }
        
        #region Movement
        Vector3 normalVector;
        public Vector3 targetPosition;

        //这个方法使玩家的运动方向和camera方向保持一致
        private void HandleRotateion(float delta)
        {
            if(inputHandler.lockOnFlag)
            {
                Vector3 targetDir = inputHandler.cameraHandler.currnetLockOnTarget.position;
                targetDir.y = 0;
                myTransform.LookAt(targetDir);
            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                targetDir = cameraObject.forward * inputHandler.vertical;
                targetDir += cameraObject.right * inputHandler.horizontal;
                targetDir.Normalize();
                targetDir.y = 0;

                if (targetDir == Vector3.zero)
                    return;

                float rs = rotationSpeed * 50;
                Quaternion tr = Quaternion.LookRotation(targetDir);
                myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, tr, rs * delta);
            }
        }

        //这个方法定义了player的移动
        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)//玩家准备滚动
                return;
            if (playerManager.isInteracting)//玩家正在交互
                return;

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (inputHandler.moveAmount < 0.5f)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, false);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }

            if (animatorHandler.canRotate)
            {
                HandleRotateion(delta);
            }
        }
        #endregion

        public void HandleRollingAndSprinting(float delta)
        {
            isRoll = animatorHandler.anim.GetBool("IsRoll");
            if (playerManager.isInteracting)
               return;

            if (inputHandler.rollFlag)
            {
                if(!playerManager.playerStates.staminaEnable)
                {
                    return;
                }
                if (inputHandler.moveAmount > 0)
                {
                    playerManager.playerStates.TakeStaminaDamage(15);
                    if (inputHandler.lockOnFlag)
                    {
                        animatorHandler.anim.SetFloat("Roll_Vertical", inputHandler.vertical);
                        animatorHandler.anim.SetFloat("Roll_Horizontal", inputHandler.horizontal);
                    } else
                    {
                        animatorHandler.anim.SetFloat("Roll_Vertical", 0);
                        animatorHandler.anim.SetFloat("Roll_Horizontal", 0);
                    }
                    animatorHandler.anim.SetBool("IsRoll",true);
                    animatorHandler.PlayTargetAnimation("Roll", true);
                }
                else
                {
                    playerManager.playerStates.TakeStaminaDamage(10);
                    animatorHandler.PlayTargetAnimation("BackStep", true);
                }
            }
        }

        public void HandleFalling(float delta,Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin,myTransform.forward,out hit, 1f))
            {
                moveDirection = Vector3.zero;
            }

            if(playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 5f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin,-Vector3.up * minimumDistanceNeededToBeginFall,Color.red,0.1f,false);
            if(Physics.Raycast(origin,-Vector3.up,out hit,minimumDistanceNeededToBeginFall,ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;
                if(playerManager.isInAir)
                {
                    if(inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("Land",true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();

                    rigidbody.velocity = vel * (movementSpeed);
                    playerManager.isInAir = true;
                }
            }
            myTransform.position = targetPosition;
        }

        public void HandleJump()
        {
            if(playerManager.isInteracting) 
                return;

            if(inputHandler.jump_Input)
            {
                animatorHandler.PlayTargetAnimation("Jump", true);

                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                }
            }
        }

        public void HandleShield()
        {
            animatorHandler.anim.SetBool("IsShield",inputHandler.lb_Input);
            if(playerManager.isInteracting)
            {
                playerManager.playerInventory.weaponSlotManager.SetLeftShieldColliderEnbale(false);
            }
            else
            {
                playerManager.playerInventory.weaponSlotManager.SetLeftShieldColliderEnbale(inputHandler.lb_Input);
            }
        }
    }
}
