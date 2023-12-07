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


#if ENABLE_CLOUD_SERVICES_ANALYTICS
    public static int playerTimesAimed = 0;
    public static float playerTimeSpentAiming = 0f;
#endif

    private IEnumerator<float> CountAimingTime()
    {
        while(isAiming)
        {
            playerTimeSpentAiming += Time.deltaTime;
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
            playerTimesAimed++;
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