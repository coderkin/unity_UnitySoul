using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CF
{
    public class InputHandler : MonoBehaviour
    {
        [HideInInspector]public float horizontal;
        [HideInInspector] public float vertical;
        [SerializeField] public float moveAmount;
        [SerializeField] public float mouseX;
        [SerializeField] public float mouseY;

        [HideInInspector] public bool b_Input;
        [HideInInspector] public bool rollbutton_hold;
        [HideInInspector] public bool a_Input;
        [HideInInspector] public bool y_Input;
        [HideInInspector] public bool x_Input;
        [HideInInspector] public bool rb_Input;
        [HideInInspector] public bool lb_Input;
        [HideInInspector] public bool rt_Input;
        [HideInInspector] public bool lt_Input;
        [HideInInspector] public bool jump_Input;
        [HideInInspector] public bool inventory_Input;

        [HideInInspector] public bool d_pad_up;
        [HideInInspector] public bool d_pad_down;
        [HideInInspector] public bool d_pad_left;
        [HideInInspector] public bool d_pad_right;

        [HideInInspector] public bool rollFlag;
        [HideInInspector] public bool comboFlag;
        [HideInInspector] public bool lockOnFlag;
        [HideInInspector] public bool sprintFlag;
        [HideInInspector] public bool inventoryFlag;
        [HideInInspector] public float rollInputTime;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        UIManager uiManager;
        public CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        Vector2 dragVector2;
        Dragme dragme;
        Vector2 onTouchVector2;
        TouchPad touchPad;
        bool isDrag;

        Text LockOnText;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();

            dragme = GameObject.Find("Player UI/MyDragBar/DragArea").GetComponent<Dragme>();
            dragme.joystickDrag += JoystickDrag;

            touchPad = GameObject.Find("Player UI/TouchPad").GetComponent<TouchPad>();
            touchPad.onTouchPad += OnTouch;

            GameObject.Find("Player UI/HUD Window/PlayerControll/Attack")
                .GetComponent<EventButton>().holdButton += OnAttackButtonHold;

            GameObject.Find("Player UI/HUD Window/PlayerControll/Shield")
                .GetComponent<EventButton>().holdButton += OnShieldButtonHold;

            GameObject.Find("Player UI/HUD Window/PlayerControll/Roll")
                .GetComponent<EventButton>().holdButton += OnRollButtonHold;

            GameObject.Find("Player UI/HUD Window/PlayerControll/Jump")
                .GetComponent<EventButton>().holdButton += OnJumpButtonHold;

            LockOnText = GameObject.Find("Player UI/HUD Window/PlayerControll/LockOn/Text")
                .GetComponent<Text>();
        }

        public void OnAttackButtonHold(bool hold)
        {
            if(hold)
            {
                rb_Input = true;
            }
        }

        public void OnShieldButtonHold(bool hold)
        {
            lb_Input = hold;
        }

        public void OnRollButtonHold(bool hold) {
            rollbutton_hold = hold;
        }

        public void OnJumpButtonHold(bool hold)
        {
            jump_Input = hold;
        }

        private void JoystickDrag(Vector2 drag, bool isDrag)
        {
            dragVector2 = drag;
            this.isDrag = isDrag;
        }

        private void OnTouch(Vector2 onTouch)
        {
            onTouchVector2 = onTouch;
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += cameraRotation => cameraInput = cameraRotation.ReadValue<Vector2>();
                inputActions.PlayerAction.RB.performed += i => rb_Input = true;
                inputActions.PlayerAction.LB.performed += i => lb_Input = true;
                inputActions.PlayerAction.LB.canceled += i => lb_Input = false;
                inputActions.PlayerAction.RT.performed += i => rt_Input = true;
                inputActions.PlayerAction.LT.performed += i => lt_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_pad_right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_pad_left = true;
                inputActions.PlayerQuickSlots.DPadUp.performed += i => d_pad_up = true;
                inputActions.PlayerQuickSlots.DPadDown.performed += i => HandleDPadDownClick();
                inputActions.PlayerAction.A.performed += inputActions => a_Input = true;
                inputActions.PlayerAction.Y.performed += y => y_Input = true;
                inputActions.PlayerAction.X.performed += y => x_Input = true;
                inputActions.PlayerAction.Jump.performed += j => jump_Input = true;
                inputActions.PlayerAction.Inventory.performed += j => HandleInventoryInput();
                inputActions.PlayerAction.LockOn.performed += lockon => HandleLockOnInput();
                inputActions.PlayerMovement.LockOnTargetLeft.performed += left => HandleLockOnTargetChange(true);
                inputActions.PlayerMovement.LockOnTargetRight.performed += right => HandleLockOnTargetChange(false);
            }
            inputActions.Enable();
        }

        public void ClearFlagOnLateUpdate()
        {
            mouseX = 0;
            mouseY = 0;
            rollFlag = false;
            rb_Input = false;
            a_Input = false;
            y_Input = false;
            x_Input = false;
            rt_Input = false;
            d_pad_up = false;
            d_pad_down = false;
            d_pad_left = false;
            d_pad_right = false;
            jump_Input = false;
            lt_Input = false;
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            CheckEnemyParryStatus();
        
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput();
            HandleQuickSlotsInput(delta);
            cameraHandler.FindLeftAndRightEnemy();
        }

        public void HandlePlayerDeath()
        {
            dragme.gameObject.SetActive(false);
        }

        public void HandleResurrection()
        {
            dragme.gameObject.SetActive(true);
        }

        private void CheckEnemyParryStatus()
        {
            
        }

        public void HandleMoveInput(float delta)
        {
#if UNITY_EDITOR
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
#else
            horizontal = dragVector2.x;
            vertical = dragVector2.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = onTouchVector2.x;
            mouseY = onTouchVector2.y;
#endif
        }

        public void HandleRollInput(float delta)
        {
            b_Input = inputActions.PlayerAction.Roll.phase == InputActionPhase.Started;

            sprintFlag = b_Input || rollbutton_hold;
            if (sprintFlag)
            {
                rollInputTime += delta;
            }else
            {
                if(rollInputTime > 0 && rollInputTime < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
                rollInputTime = 0;
            }
        }

        public void HandleAttackInput()
        {
            if (!playerManager.playerStates.staminaEnable)
                return;

            if (rb_Input)
            {
                if(!playerManager.isInteracting && playerManager.playerStates.parryEnemy != null && playerManager.playerStates.parryEnemy.getParried)
                {
                    playerManager.isInteracting = true;
                    playerAttacker.HandleParryAttack(playerInventory.rightWeapon);
                    return;
                }

                if(playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }else
                {
                    if (playerManager.canDoCombo)
                        return;
                    if (playerManager.isInteracting)
                        return;

                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleQuickSlotsInput(float delta)
        {
            if (d_pad_right)
            {
                playerInventory.ChangeRightWeapon();
            }else if(d_pad_left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            uiManager.UpdateUI();
            bool isActive = uiManager.selectWindow.activeInHierarchy;
            bool inventoryActive = uiManager.weaponInventoryWindow.activeInHierarchy;
            bool equipmentActive = uiManager.equipmentWindowUI.gameObject.activeInHierarchy;
            bool consumableActive = uiManager.consumableInventoryWindowUI.gameObject.activeInHierarchy;

            if (inventoryActive || equipmentActive || consumableActive)
            {
                uiManager.CloseAllInventoryWindow();
                uiManager.hudWindow.SetActive(true);
                uiManager.OpenOrCloseSelectWindow(false);
            }
            else
            {
                uiManager.OpenOrCloseSelectWindow(!isActive);
                uiManager.hudWindow.SetActive(isActive);
            }
        }

        public void HandleLockOnInput()
        {
            if(!lockOnFlag)
            {
                cameraHandler.HandleLockOn();
            }else
            {
                cameraHandler.ClearLockOnTargets();
            }

            if(lockOnFlag)
            {
                LockOnText.text = "解锁";
            }
            else
            {
                LockOnText.text = "锁定";
            }
        }

        private void HandleLockOnTargetChange(bool isLeft)
        {
            cameraHandler.HandleLockOnChange(isLeft);
        }

        private void HandleDPadDownClick()
        {
            playerInventory.ChangeConsumableItem();
        }
    }
}
