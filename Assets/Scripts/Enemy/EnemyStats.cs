using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class EnemyStats : CharacterStats
    {
        public Animator animator;
        public EnemyManager enemyManager;
        public EnemyHealthBar enemyHealthBar;

        public bool getParried;
        public PlayerStats attackPlayer;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            enemyManager = GetComponent<EnemyManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int demage,string animName = "Demage_1")
        {
            currentHealth = currentHealth - demage;
            if (currentHealth <= 0)
            {
                animator.CrossFade("Death", 0.2f);
                currentHealth = 0;
            }else
            {
                animator.CrossFade(animName, 0.2f);
            }
            enemyHealthBar.SetCurrentHealth(currentHealth);
        }

        public void TakeDamageNoAnim(int demage)
        {
            currentHealth = currentHealth - demage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }
            enemyHealthBar.SetCurrentHealth(currentHealth);
        }

        public void BeParried(bool isParried)
        {
            getParried = isParried;
        }

        public void RestartGame()
        {
            if(currentHealth <= 0)
            {
                animator.CrossFade("Get_Up", 0.2f);
            }

            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
        }
    }
}
