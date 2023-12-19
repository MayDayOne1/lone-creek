using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class TriggerVictory : MonoBehaviour
{
    [SerializeField] private ASyncLoader asyncLoader;
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
                { "playerHealth", PlayerParams.health },
                { "playerHealthKitCount", PlayerParams.playerHealthKitCount },
                { "playerDeathCount", PlayerParams.playerDeathCount },
                { "playerPistolAmmo", PlayerParams.currentAmmo + PlayerParams.currentClip },
                { "playerAmmoClipCount", PlayerParams.playerAmmoClipCount },
                { "playerBottleCount",  PlayerParams.playerBottleCount },
                { "playerBottleThrowCount", PlayerParams.playerBottleThrowCount },
                { "playerShotsFiredCount", PlayerParams.playerShotsFiredCount },
                { "level2TimeSpent", controller.level2TimeSpent },
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
        }
    }

    public void OnReturnToMainMenu()
    {
        asyncLoader.LoadLevel(0);
        SurveyManager.isActive = true;
    }
}
