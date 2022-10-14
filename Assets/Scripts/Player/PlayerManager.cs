using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CF {
    public class PlayerManager : CharacterManager
    {
        [HideInInspector]
        public AnimatorEventHandler animatorEventHandler;
        [HideInInspector]
        public InputHandler inputHandler;
        [HideInInspector]
        public CameraHandler cameraHandler;
        [HideInInspector]
        public PlayerLocomotion playerLocomotion;
        [HideInInspector]
        public PlayerInventory playerInventory;
        [HideInInspector]
        public PlayerAttacker playerAttacker;
        [HideInInspector]
        public InteractableUI interctableUI;
        [HideInInspector]
        public PlayerStats playerStates;

        public UIManager uiManager;
        public GameObject interactableUIGameObject;
        public GameObject itemPopUIGameObject;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;

        public bool canDoCombo;

        public bool gameOver;
        public int resurrectionCount = 0;
        public Text resurrectionCountText;

        RaycastHit hit;

        protected override void Awake()
        {
            base.Awake();
            cameraHandler = CameraHandler.singleton;
            animatorEventHandler = GetComponentInChildren<AnimatorEventHandler>();
            inputHandler = GetComponent<InputHandler>();
            interctableUI = FindObjectOfType<InteractableUI>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerStates = GetComponent<PlayerStats>();
        }

        protected override void Update()
        {
            if (gameOver)
            {
                if (inputHandler.jump_Input)
                    HandleResurrection();
                return;
            }

            base.Update();
            float delta = Time.deltaTime;
            canDoCombo = animatorManager.anim.GetBool("CanDoCombo");
            animatorManager.anim.SetBool("IsInAir", isInAir);
            playerStates.isParry = animatorManager.anim.GetBool("IsParry");
            inputHandler.TickInput(delta);

            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJump();
            playerLocomotion.HandleShield();
            CheckForInteractableObject();
            HandleConsumable();
            playerInventory.Tick();
            if (!isInteracting)
            {
                playerStates.RecoverStamina(Time.deltaTime);
            }

            if (inputHandler.lt_Input)
            {
                playerAttacker.HandleParry();
            }
        }

        private void FixedUpdate()
        {
            if (gameOver)
            {
                return;
            }
            float delta = Time.fixedDeltaTime;
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

        }

        private void LateUpdate()
        {
            HandleCameraRotate(Time.deltaTime);

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }

            inputHandler.ClearFlagOnLateUpdate();
        }

        private void HandleCameraRotate(float delta)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }

        public void CheckForInteractableObject()
        {
            if (cameraHandler == null)
            {
                cameraHandler = CameraHandler.singleton;
            }

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, LayerMask.GetMask("TransparentFX")))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                    if (interactableObject != null)
                    {
                        string text = interactableObject.interctableText;
                        interctableUI.interactText.text = text;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                        
                        interctableUI.interactBtn.onClick.AddListener(InteractButtonClick);
                    }
                }
            } else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemPopUIGameObject != null && inputHandler.a_Input)
                {
                    itemPopUIGameObject.SetActive(false);
                }
            }
        }

        public void InteractButtonClick()
        {
            hit.collider.GetComponent<Interactable>().Interact(this);
        }

        private void HandleConsumable()
        {
            if(inputHandler.y_Input && !isInteracting)
            {
                playerAttacker.HandleConsumable();
            }
        }

        public void HandlePlayerDeath()
        {
            inputHandler.ClearFlagOnLateUpdate();
            uiManager.gameOverWindow.SetActive(true);
            inputHandler.HandlePlayerDeath();
            gameOver = true;
        }

        public void HandleResurrection()
        {
            resurrectionCount++;
            resurrectionCountText.text = "复活次数：" + resurrectionCount;
            uiManager.gameOverWindow.SetActive(false);
            inputHandler.HandleResurrection();

            gameOver = false;

            //血量恢复
            playerStates.FullHealth();
            //站起来
            animatorManager.PlayTargetAnimation("Resurrection",true);
        }

        public void GameOver()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void RestartGame()
        {
            resurrectionCount = 0;
            resurrectionCountText.text = "复活次数：" + resurrectionCount;

            inputHandler.HandleResurrection();
            gameOver = false;
            //血量恢复
            playerStates.FullHealth();

            transform.position = new Vector3(-3.29f,0.44f,8.99f);
            transform.rotation = Quaternion.identity;

            cameraHandler.RestartGame();

            EnemyManager em = GameObject.Find("Enemys/Enemy1").GetComponent<EnemyManager>();
            em.RestartGame();
        }
    }
}
