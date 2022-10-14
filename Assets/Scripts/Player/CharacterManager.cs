using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class CharacterManager : MonoBehaviour
    {
        [HideInInspector]
        public AnimatorManager animatorManager;
        [HideInInspector]
        public bool isInteracting;

        public Transform lockOnTransform;

        protected virtual void Awake()
        {
            animatorManager = GetComponentInChildren<AnimatorManager>();
        }

        protected virtual void Update()
        {
            isInteracting = animatorManager.anim.GetBool("IsInteracting");
        }
    }
}
