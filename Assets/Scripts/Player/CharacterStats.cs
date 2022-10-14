using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public bool IsAlive()
        {
            return currentHealth > 0;
        }
    }
}
