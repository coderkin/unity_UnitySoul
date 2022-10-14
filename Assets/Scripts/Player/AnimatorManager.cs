using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        int vertical;
        int horizontal;
        public bool canRotate;

        public virtual void Initialize()
        { 
            anim = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public virtual void UpdateAnimatorValues(float verticalMovement,float horizontalMovement,bool isSprinting)
        {
            #region Vertical
            if(verticalMovement > 0 && verticalMovement < 0.55f)
            {
                verticalMovement = 0.5f;
            }else if(verticalMovement > 0.55f)
            {
                verticalMovement = 1;
            }else if(verticalMovement < 0 && verticalMovement > -0.55f)
            {
                verticalMovement = -0.5f;
            }else if(verticalMovement < -0.55f)
            {
                verticalMovement = -1;
            }else
            {
                verticalMovement = 0;
            }
            #endregion


            #region Horizontal
            float h = horizontalMovement;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                horizontalMovement = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                horizontalMovement = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                horizontalMovement = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                horizontalMovement = -1;
            }
            else
            {
                horizontalMovement = 0;
            }
            #endregion

            if (isSprinting)
            {
                verticalMovement = 2;
                horizontalMovement = h;
            }
            anim.SetFloat(vertical,verticalMovement,0.1f,Time.deltaTime);
            anim.SetFloat(horizontal,horizontalMovement,0.1f,Time.deltaTime);
        }

        public virtual void PlayTargetAnimation(string targetAnim,bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("IsInteracting",isInteracting);
            anim.CrossFade(targetAnim,0.2f);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public void EnableCombo()
        {
            anim.SetBool("CanDoCombo",true);
        }

        public void DisableCombo()
        {
            anim.SetBool("CanDoCombo", false);
        }

    }
}
