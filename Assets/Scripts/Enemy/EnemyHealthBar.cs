using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CF
{
    public class EnemyHealthBar : MonoBehaviour
    {
        Slider healthBar;

        private void Awake()
        {
            healthBar = GetComponent<Slider>();
        }

        public void SetMaxHealth(float health)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }

        public void SetCurrentHealth(float health)
        {
            Debug.Log(health);
            healthBar.value = health;
        }
    }
}
