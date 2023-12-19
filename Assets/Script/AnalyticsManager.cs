#if ENABLE_CLOUD_SERVICES_ANALYTICS

using System.Collections.Generic;
using Unity.Services.Analytics;
public class AnalyticsManager
{
    public const string LEVEL_1_COMPLETED = "level1Completed";
    public const string LEVEL_2_COMPLETED = "level2Completed";
    public const string ONBOARDING_COMPLETED = "onboardingCompleted";
    public const string LEVEL_1_QUIT = "level1quit";
    public const string LEVEL_2_QUIT = "level2quit";
    public const string PLAYER_DIE = "playerDie";
    public const string ENEMY_DIE = "enemyDie";
    public const string PLAYER_REACHED_CABIN = "playerReachedCabin";
    public const string PLAYER_REACHED_BARN = "playerReachedBarn";
    public const string PLAYER_REACHED_GAS = "playerReachedGas";

    public const string ENEMIES_KILLED = "enemiesKilled";
    public const string ENEMY_SHOTS_FIRED_COUNT = "enemyShotsFiredCount";
    public const string ENEMY_SHOTS_HIT = "enemyShotsHit";
    public const string LEVEL_1_TIME_SPENT = "level1TimeSpent";
    public const string LEVEL_2_TIME_SPENT = "level2TimeSpent";
    public const string ONBOARDING_TIME_SPENT = "onboardingTimeSpent";
    public const string PLAYER_AMMO_CLIP_COUNT = "playerAmmoClipCount";
    public const string PLAYER_BOTTLE_COUNT = "playerBottleCount";
    public const string PLAYER_BOTTLE_THROW_COUNT = "playerBottleThrowCount";
    public const string PLAYER_DEATH_COUNT = "playerDeathCount";
    public const string PLAYER_HEALTH = "playerHealth";
    public const string PLAYER_HEALTH_KIT_COUNT = "playerHealthKitCount";
    public const string PLAYER_PISTOL_AMMO = "playerPistolAmmo";
    public const string PLAYER_PISTOLS_PICKED_UP = "playerPistolsPickedUp";
    public const string PLAYER_SHOTS_FIRED_COUNT = "playerShotsFiredCount";
    public const string PLAYER_SHOTS_HIT = "playerShotsHit";
    public const string PLAYER_TIME_SPENT_AIMING = "playerTimeSpentAiming";
    public const string PLAYER_TIME_SPENT_CROUCHING = "playerTimeSpentCrouching";
    public const string PLAYER_TIME_SPENT_STANDING = "playerTimeSpentStanding";
    public const string PLAYER_TIMES_AIMED = "playerTimesAimed";
    public const string PLAYER_TIMES_CROUCHED = "playerTimesCrouched";
    public const string PLAYER_TIMES_DETECTED = "playerTimesDetected";


    public void SendEvent(string name, Dictionary<string, object> dictionary)
    {
        AnalyticsService.Instance.CustomData(name, dictionary);
    }
}
#endif
