using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MEC;

public class PlayerShootingManager : MonoBehaviour
{
    public IWeapon currentWeapon = null;
    public IWeapon previousWeapon = null;

    public bool isAiming = false;
    public bool isPistolEquipped = false;

    private IEnumerator<float> CountAimingTime()
    {
        while(isAiming)
        {
            PlayerParams.playerTimeSpentAiming += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }

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
}