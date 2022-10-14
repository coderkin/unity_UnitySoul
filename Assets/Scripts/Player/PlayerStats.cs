using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class PlayerStats : CharacterStats
    {
        public int staminaLevel = 10;
        public bool staminaEnable = true;
        public int recoverStaminaBase = 2;
        public int maxStamina;
        public float currentStamina;

        public bool isParry;

        HealthBar healthbar;
        StaminaBar staminabar;
        PlayerAnimatorManager animatorHandler;
        public PlayerManager playerManager;
        public bool IsImmune;

        [HideInInspector]
        public EnemyStats parryEnemy;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            healthbar = FindObjectOfType<HealthBar>();
            staminabar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            FullHealth();

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminabar.SetMaxStamina(maxStamina);
        }

        public void FullHealth()
        {
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(currentHealth);
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            if(IsImmune)
            {
                IsImmune = false;
                return;
            }
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                animatorHandler.PlayTargetAnimation("Death", true);
                currentHealth = 0;
                playerManager.HandlePlayerDeath();
            }
            else
            {
                if (!playerManager.playerLocomotion.isRoll)
                    animatorHandler.PlayTargetAnimation("Demage_1",true);
            }
            healthbar.SetCurrentHealth(currentHealth);
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            if(currentStamina <= 0)
            {
                staminaEnable = false;
                staminabar.EnableStaminaBar(staminaEnable);
                currentStamina = 0;
            }
            staminabar.SetCurrentStamina(currentStamina);
        }

        public void RecoverStamina(float delta)
        {
            if(currentStamina < maxStamina)
            {
                currentStamina += delta * (staminaLevel * recoverStaminaBase);
                if (currentStamina > maxStamina)
                {
                    staminaEnable = true;
                    staminabar.EnableStaminaBar(staminaEnable);
                    currentStamina = maxStamina;
                }
                staminabar.SetCurrentStamina(currentStamina);
            }
        }
    }
}
