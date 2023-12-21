using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MEC;

public class PlayerShootingManager : MonoBehaviour
{
    public IWeapon currentWeapon = null;
    public IWeapon previousWeapon = null;

    public ThrowableWeapon throwableWeapon;
    public PistolWeapon pistolWeapon;

    public bool isAiming = false;
    public bool isPistolEquipped = false;

    public void Aim(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(currentWeapon != null)
            {
                currentWeapon.StartAim();
                isAiming = true;
            }

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            PlayerParams.playerTimesAimed++;
            Timing.RunCoroutine(CountAimingTime());
#endif
        }
        else if(context.canceled)
        {
            if (currentWeapon != null)
            {
                currentWeapon.StopAim();
                isAiming = false;
            }

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            StopCoroutine(CountAimingTime());
#endif
        }
    }
    public void Shoot()
    {
        currentWeapon?.Shoot();
    }

    public void SelectThrowable()
    {
        currentWeapon?.Disable();
        if (PlayerParams.hasThrowable)
        {
            currentWeapon = throwableWeapon;
            currentWeapon.Select();
        }

    }

    public void SelectPrimary()
    {
        currentWeapon?.Disable();
        if (PlayerParams.hasPrimary)
        {
            currentWeapon = pistolWeapon;
            currentWeapon.Select();
        }
    }

    private IEnumerator<float> CountAimingTime()
    {
        while (isAiming)
        {
            PlayerParams.playerTimeSpentAiming += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }
}