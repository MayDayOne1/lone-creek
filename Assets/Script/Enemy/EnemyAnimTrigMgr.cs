using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimTrigMgr : StateMachineBehaviour
{
    private AI ai;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = FindFirstObjectByType<AI>();
        if (ai.GetCanAttackPlayer())
        {
            // Debug.Log("Can attack player");
            animator.SetTrigger("Shoot");
        }
        else
        {
            // Debug.Log("Cannot attack player");
            animator.ResetTrigger("Shoot");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    ai = FindFirstObjectByType<AI>();
    //    if (ai.GetCanAttackPlayer())
    //    {
    //        Debug.Log("Can attack player");
    //        animator.SetTrigger("Shoot");
    //    }
    //    else
    //    {
    //        Debug.Log("Cannot attack player");
    //        animator.ResetTrigger("Shoot");
    //    }
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
