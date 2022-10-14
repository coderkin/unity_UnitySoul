using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class NapalmBombDamageCollider : ConsumableDamageCollider
    {
        private float takeDamageTime = 2;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == 9)
            {
                damageConsumableItem.PlayDamageFX();
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            EnemyStats enemyStats = other.transform.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                if (takeDamageTime >= 1) {
                    enemyStats.TakeDamage(Mathf.RoundToInt(damageConsumableItem.baseDamage));
                    takeDamageTime = 0;
                }else
                {
                    takeDamageTime += Time.deltaTime;
                }
            }
        }
    }
}
