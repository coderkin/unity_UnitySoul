using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolOnStateEnter : StateMachineBehaviour
{
    public string parametersName;
    public bool value;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(parametersName, value);
    }
}
