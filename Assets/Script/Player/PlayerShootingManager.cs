using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using System.Collections;

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

    private IEnumerator CountAimingTime()
    {
        playerTimeSpentAiming += Time.deltaTime;
        yield return null;
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
            StartCoroutine(CountAimingTime());
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