using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class PlayerEnterGas : MonoBehaviour
{
    private bool hasEntered = false;
    void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null && !hasEntered)
        {
            hasEntered = true;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            AnalyticsService.Instance.CustomData("playerReachedGas", new Dictionary<string, object>()
            {
                { "enemiesKilled", PlayerController.enemiesKilled },
                { "enemyShotsFiredCount", PlayerController.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerController.enemyShotsHit },
                { "level2TimeSpent", controller.level2TimeSpent },
                { "playerAmmoClipCount", PlayerInteract.playerAmmoClipCount },
                { "playerBottleCount",  PlayerInteract.playerBottleCount },
                { "playerBottleThrowCount", ThrowableWeapon.playerBottleThrowCount },
                { "playerHealth", PlayerController.health },
                { "playerHealthKitCount", PlayerController.playerHealthKitCount },
                { "playerPistolAmmo", PlayerAmmoManager.currentAmmo + PlayerAmmoManager.currentClip },
                { "playerPistolsPickedUp", PlayerInteract.playerPistolsPickedUp },
                { "playerShotsFiredCount", PistolWeapon.playerShotsFiredCount },
                { "playerShotsHit", PistolWeapon.playerShotsHit },
                { "playerTimesAimed", PlayerShootingManager.playerTimesAimed },
                { "playerTimesCrouched", PlayerController.playerTimesCrouched },
                { "playerTimesDetected", PlayerController.playerTimesDetected },
                { "playerTimeSpentAiming", PlayerShootingManager.playerTimeSpentAiming },
                { "playerTimeSpentCrouching", PlayerController.playerTimeSpentCrouching },
                { "playerTimeSpentStanding", PlayerController.playerTimeSpentStanding }
            });
#endif
        }
    }
}
