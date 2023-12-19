using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.Services.Analytics;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

public class LoadDesert : MonoBehaviour
{
    [SerializeField] private ASyncLoader asyncLoader;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.StopCoroutine(controller.level1coroutine);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            AnalyticsService.Instance.CustomData("level1Completed", new Dictionary<string, object>()
            {
                { "playerHealth", PlayerParams.health },
                { "playerHealthKitCount", PlayerParams.playerHealthKitCount },
                { "playerDeathCount", PlayerParams.playerDeathCount },
                { "playerPistolAmmo", PlayerParams.currentAmmo + PlayerParams.currentClip },
                { "playerAmmoClipCount", PlayerParams.playerAmmoClipCount },
                { "playerBottleCount",  PlayerParams.playerBottleCount },
                { "playerBottleThrowCount", PlayerParams.playerBottleThrowCount },
                { "playerShotsFiredCount", PlayerParams.playerShotsFiredCount },
                { "level1TimeSpent", controller.level1TimeSpent },
                { "enemiesKilled", PlayerParams.enemiesKilled },
                { "enemyShotsFiredCount", PlayerParams.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerParams.enemyShotsHit },
                { "playerPistolsPickedUp", PlayerParams.playerPistolsPickedUp },
                { "playerShotsHit", PlayerParams.playerShotsHit },
                { "playerTimesAimed", PlayerParams.playerTimesAimed },
                { "playerTimeSpentAiming", PlayerParams.playerTimeSpentAiming },
                { "playerTimesCrouched", PlayerParams.playerTimesCrouched },
                { "playerTimesDetected", PlayerParams.playerTimesDetected },
                { "playerTimeSpentCrouching", PlayerParams.playerTimeSpentCrouching },
                { "playerTimeSpentStanding", PlayerParams.playerTimeSpentStanding }
            });
#endif
            asyncLoader.LoadLevel(2);
        }
    }
}
