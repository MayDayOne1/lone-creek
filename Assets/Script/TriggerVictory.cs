using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class TriggerVictory : MonoBehaviour
{
    public GameObject VictoryScreen;
    public PlayerController controller;

    private void Start()
    {
        VictoryScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            VictoryScreen.SetActive(true);
            controller.VictorySetup();
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            AnalyticsService.Instance.CustomData("level2Completed", new Dictionary<string, object>()
            {
                { "playerHealth", PlayerController.health },
                { "playerHealthKitCount", PlayerController.playerHealthKitCount },
                { "playerDeathCount", PlayerController.playerDeathCount },
                { "playerPistolAmmo", PlayerAmmoManager.currentAmmo + PlayerAmmoManager.currentClip },
                { "playerAmmoClipCount", PlayerInteract.playerAmmoClipCount },
                { "playerBottleCount",  PlayerInteract.playerBottleCount },
                { "playerBottleThrowCount", PlayerShootingManager.playerBottleThrowCount },
                { "playerShotsFiredCount", PlayerShootingManager.playerShotsFiredCount },
                { "level2TimeSpent", controller.level2TimeSpent },
                { "enemiesKilled", PlayerController.enemiesKilled },
                { "enemyShotsFiredCount", PlayerController.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerController.enemyShotsHit },
                { "playerPistolsPickedUp", PlayerInteract.playerPistolsPickedUp },
                { "playerShotsHit", PlayerShootingManager.playerShotsHit },
                { "playerTimesAimed", PlayerShootingManager.playerTimesAimed },
                { "playerTimeSpentAiming", PlayerShootingManager.playerTimeSpentAiming },
                { "playerTimesCrouched", PlayerController.playerTimesCrouched },
                { "playerTimeSpentCrouching", PlayerController.playerTimeSpentCrouching },
                { "playerTimeSpentStanding", PlayerController.playerTimeSpentStanding }
            });
#endif
        }
    }

    public void OnReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
