using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif


public class PauseMenuManager : MonoBehaviour
{
    public PlayerController controller;
    public void OnResume()
    {
        controller.TogglePauseMenu();
    }

    public void OnQuit()
    {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            controller.StopCoroutine(controller.level1coroutine);
            AnalyticsService.Instance.CustomData("level1quit", new Dictionary<string, object>()
            {
                { "enemiesKilled", PlayerController.enemiesKilled },
                { "enemyShotsFiredCount", PlayerController.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerController.enemyShotsHit },
                { "level1TimeSpent", controller.level1TimeSpent },
                { "playerAmmoClipCount", PlayerInteract.playerAmmoClipCount },
                { "playerBottleCount",  PlayerInteract.playerBottleCount },
                { "playerBottleThrowCount", PlayerShootingManager.playerBottleThrowCount },
                { "playerDeathCount", PlayerController.playerDeathCount },
                { "playerHealth", PlayerController.health },
                { "playerHealthKitCount", PlayerController.playerHealthKitCount },
                { "playerPistolAmmo", PlayerAmmoManager.currentAmmo + PlayerAmmoManager.currentClip },
                { "playerPistolsPickedUp", PlayerInteract.playerPistolsPickedUp },
                { "playerShotsFiredCount", PlayerShootingManager.playerShotsFiredCount },
                { "playerShotsHit", PlayerShootingManager.playerShotsHit },
                { "playerTimesAimed", PlayerShootingManager.playerTimesAimed },
                { "playerTimesCrouched", PlayerController.playerTimesCrouched },
                { "playerTimeSpentAiming", PlayerShootingManager.playerTimeSpentAiming },
                { "playerTimeSpentCrouching", PlayerController.playerTimeSpentCrouching },
                { "playerTimeSpentStanding", PlayerController.playerTimeSpentStanding }
            });
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            controller.StopCoroutine(controller.level2coroutine);
            AnalyticsService.Instance.CustomData("level2quit", new Dictionary<string, object>()
            {
                { "enemiesKilled", PlayerController.enemiesKilled },
                { "enemyShotsFiredCount", PlayerController.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerController.enemyShotsHit },
                { "level2TimeSpent", controller.level2TimeSpent },
                { "playerAmmoClipCount", PlayerInteract.playerAmmoClipCount },
                { "playerBottleCount",  PlayerInteract.playerBottleCount },
                { "playerBottleThrowCount", PlayerShootingManager.playerBottleThrowCount },
                { "playerDeathCount", PlayerController.playerDeathCount },
                { "playerHealth", PlayerController.health },
                { "playerHealthKitCount", PlayerController.playerHealthKitCount },
                { "playerPistolAmmo", PlayerAmmoManager.currentAmmo + PlayerAmmoManager.currentClip },
                { "playerPistolsPickedUp", PlayerInteract.playerPistolsPickedUp },
                { "playerShotsFiredCount", PlayerShootingManager.playerShotsFiredCount },
                { "playerShotsHit", PlayerShootingManager.playerShotsHit },
                { "playerTimesAimed", PlayerShootingManager.playerTimesAimed },
                { "playerTimesCrouched", PlayerController.playerTimesCrouched },
                { "playerTimeSpentAiming", PlayerShootingManager.playerTimeSpentAiming },
                { "playerTimeSpentCrouching", PlayerController.playerTimeSpentCrouching },
                { "playerTimeSpentStanding", PlayerController.playerTimeSpentStanding }
            });
        }
#endif
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OnTryAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
