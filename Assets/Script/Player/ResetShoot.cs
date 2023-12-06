using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetShoot : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Reload");
    }
}
