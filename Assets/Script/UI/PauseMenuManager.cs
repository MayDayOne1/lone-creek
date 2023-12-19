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
                { "enemiesKilled", PlayerParams.enemiesKilled },
                { "enemyShotsFiredCount", PlayerParams.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerParams.enemyShotsHit },
                { "level1TimeSpent", controller.level1TimeSpent },
                { "playerAmmoClipCount", PlayerParams.playerAmmoClipCount },
                { "playerBottleCount",  PlayerParams.playerBottleCount },
                { "playerBottleThrowCount", PlayerParams.playerBottleThrowCount },
                { "playerDeathCount", PlayerParams.playerDeathCount },
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
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            controller.StopCoroutine(controller.level2coroutine);
            AnalyticsService.Instance.CustomData("level2quit", new Dictionary<string, object>()
            {
                { "enemiesKilled", PlayerParams.enemiesKilled },
                { "enemyShotsFiredCount", PlayerParams.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerParams.enemyShotsHit },
                { "level2TimeSpent", controller.level2TimeSpent },
                { "playerAmmoClipCount", PlayerParams.playerAmmoClipCount },
                { "playerBottleCount",  PlayerParams.playerBottleCount },
                { "playerBottleThrowCount", PlayerParams.playerBottleThrowCount },
                { "playerDeathCount", PlayerParams.playerDeathCount },
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
        }
#endif
        Time.timeScale = 1f;
        SurveyManager.isActive = true;
        SceneManager.LoadScene(0);
    }

    public void OnTryAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
