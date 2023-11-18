using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
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
        if (SceneManager.GetActiveScene().name == "SceneTunnel")
        {
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
        else if (SceneManager.GetActiveScene().name == "SceneDesert")
        {
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
            SceneManager.LoadScene("MainMenu");
    }

    public void OnTryAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }
}
