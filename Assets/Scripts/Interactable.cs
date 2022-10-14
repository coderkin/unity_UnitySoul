using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class Interactable : MonoBehaviour
    {
        public string interctableText;

        public virtual void Interact(PlayerManager playerManager)
        {
            //Called when Player Pickup
        }
    }
}
