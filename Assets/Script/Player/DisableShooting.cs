using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DisableShooting : StateMachineBehaviour
{
    PistolWeapon pistolWeapon;
    [Inject] private PlayerAnimManager animManager;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pistolWeapon = FindFirstObjectByType<PistolWeapon>();
        pistolWeapon.CanShoot = false;
        pistolWeapon.isReloading = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pistolWeapon = FindFirstObjectByType<PistolWeapon>();
        pistolWeapon.CanShoot = true;
        pistolWeapon.isReloading = false;
        animManager.DisableReloadLayer();
    }
}
