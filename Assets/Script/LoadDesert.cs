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
                { "playerHealth", PlayerController.health },
                { "playerHealthKitCount", PlayerController.playerHealthKitCount },
                { "playerDeathCount", PlayerController.playerDeathCount },
                { "playerPistolAmmo", PlayerAmmoManager.currentAmmo + PlayerAmmoManager.currentClip },
                { "playerAmmoClipCount", PlayerInteract.playerAmmoClipCount },
                { "playerBottleCount",  PlayerInteract.playerBottleCount },
                { "playerBottleThrowCount", ThrowableWeapon.playerBottleThrowCount },
                { "playerShotsFiredCount", PistolWeapon.playerShotsFiredCount },
                { "level1TimeSpent", controller.level1TimeSpent },
                { "enemiesKilled", PlayerController.enemiesKilled },
                { "enemyShotsFiredCount", PlayerController.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerController.enemyShotsHit },
                { "playerPistolsPickedUp", PlayerInteract.playerPistolsPickedUp },
                { "playerShotsHit", PistolWeapon.playerShotsHit },
                { "playerTimesAimed", PlayerShootingManager.playerTimesAimed },
                { "playerTimeSpentAiming", PlayerShootingManager.playerTimeSpentAiming },
                { "playerTimesCrouched", PlayerController.playerTimesCrouched },
                { "playerTimeSpentCrouching", PlayerController.playerTimeSpentCrouching },
                { "playerTimeSpentStanding", PlayerController.playerTimeSpentStanding }
            });
#endif
            asyncLoader.LoadLevel(2);
        }
    }
}
