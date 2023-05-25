using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetShoot : StateMachineBehaviour
{
    private PlayerInteract playerInteract;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // playerInteract = FindFirstObjectByType<PlayerInteract>();
        animator.ResetTrigger("Reload");
        //if (!playerInteract.Throwable.activeSelf)
        //{
        //    animator.SetLayerWeight(2, 0);
        //}
    }
}
