using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF {
    public class BoomDamageCollider : ConsumableDamageCollider
    {
        private void OnCollisionEnter(Collision collision)
        {
            EnemyStats enemyStats = collision.transform.GetComponent<EnemyStats>();
            if(enemyStats != null)
            {
                enemyStats.TakeDamage(Mathf.RoundToInt(damageConsumableItem.baseDamage));
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, damageConsumableItem.boomRange, LayerMask.GetMask("Character"));
            foreach (var collider in colliders)
            {
                EnemyStats es = collider.transform.GetComponent<EnemyStats>();
                if(es != null)
                {
                    es.TakeDamage(Mathf.RoundToInt(damageConsumableItem.rangeDamage));
                }
            }

            //撞到东西就销毁自己，并播放爆炸特效
            damageConsumableItem.PlayDamageFX();
            Destroy(gameObject);
        }
    }
}
