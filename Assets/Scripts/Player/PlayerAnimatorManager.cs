using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        public PlayerManager playerManager;
        PlayerLocomotion playerLocomotion;

        public override void Initialize()
        {
            base.Initialize();
            playerManager = GetComponentInParent<PlayerManager>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}
