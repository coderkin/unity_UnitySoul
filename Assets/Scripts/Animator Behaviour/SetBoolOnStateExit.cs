using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolOnStateExit : StateMachineBehaviour
{
    public string parametersName;
    public bool value;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(parametersName, value);
    }
}
