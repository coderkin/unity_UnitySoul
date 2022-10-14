using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF {
    public class AnimatorEventHandler : MonoBehaviour
    {
        public PlayerManager playerManager;
        public ConsumableItem consumableItem;

        public void OnAnimatorAttackConsumableThrow()
        {
            DamageConsumableItem dci = consumableItem as DamageConsumableItem;
            if (dci != null)
            {
                dci.HandleThrow();
            }
        }
        
        public void EnableParry()
        {
            playerManager.animatorManager.anim.SetBool("IsParry",true);
        }

        public void DisableParry()
        {
            playerManager.animatorManager.anim.SetBool("IsParry", false);
        }

        public void ImmuneDamage()
        {
            playerManager.playerStates.IsImmune = true;
        }

        public void NotImmuneDamage()
        {
            playerManager.playerStates.IsImmune = false;
        }
    }
}
