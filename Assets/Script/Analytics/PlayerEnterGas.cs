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
                { "enemiesKilled", PlayerParams.enemiesKilled },
                { "enemyShotsFiredCount", PlayerParams.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerParams.enemyShotsHit },
                { "level2TimeSpent", controller.level2TimeSpent },
                { "playerAmmoClipCount", PlayerParams.playerAmmoClipCount },
                { "playerBottleCount",  PlayerParams.playerBottleCount },
                { "playerBottleThrowCount", PlayerParams.playerBottleThrowCount },
                { "playerHealth", PlayerParams.health },
                { "playerHealthKitCount", PlayerParams.playerHealthKitCount },
                { "playerPistolAmmo", PlayerParams.currentAmmo + PlayerParams.currentClip },
                { "playerPistolsPickedUp", PlayerParams.playerPistolsPickedUp },
                { "playerShotsFiredCount", PlayerParams.playerShotsFiredCount },
                { "playerShotsHit", PlayerParams.playerShotsHit },
                { "playerTimesAimed", PlayerParams.playerTimesAimed },
                { "playerTimesCrouched", PlayerParams.playerTimesCrouched },
                { "playerTimesDetected", PlayerParams.playerTimesDetected },
                { "playerTimeSpentAiming", PlayerParams.playerTimeSpentAiming },
                { "playerTimeSpentCrouching", PlayerParams.playerTimeSpentCrouching },
                { "playerTimeSpentStanding", PlayerParams.playerTimeSpentStanding }
            });
#endif
        }
    }
}
