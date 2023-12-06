using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableShooting : StateMachineBehaviour
{
    PistolWeapon pistolWeapon;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pistolWeapon = FindFirstObjectByType<PistolWeapon>();
        pistolWeapon.CanShoot = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pistolWeapon = FindFirstObjectByType<PistolWeapon>();
        pistolWeapon.CanShoot = true;
    }
}
