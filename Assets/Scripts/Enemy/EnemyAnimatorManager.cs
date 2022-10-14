using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyManager enemyManager;

        private void Awake()
        {
            enemyManager = GetComponentInParent<EnemyManager>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void OnAnimatorMove()
        {
            if (enemyManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            enemyManager.mRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.mRigidbody.velocity = velocity;
        }

        public void GettingParried()
        {
            StartCoroutine(SetParried());
        }

        IEnumerator SetParried()
        {
            enemyManager.enemyStats.BeParried(true);
            yield return new WaitForSeconds(2f);
            enemyManager.enemyStats.BeParried(false);
        }
    }
}
