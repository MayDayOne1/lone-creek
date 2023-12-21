#if ENABLE_CLOUD_SERVICES_ANALYTICS

using System.Collections.Generic;
using Unity.Services.Analytics;
using Zenject;

public class AnalyticsManager
{
    #region EVENTS
    private const string LEVEL_1_COMPLETED = "level1Completed";
    private const string LEVEL_2_COMPLETED = "level2Completed";
    private const string ONBOARDING_COMPLETED = "onboardingCompleted";
    private const string LEVEL_1_QUIT = "level1quit";
    private const string LEVEL_2_QUIT = "level2quit";
    private const string PLAYER_DIE = "playerDie";
    private const string ENEMY_DIE = "enemyDie";
    private const string PLAYER_REACHED_CABIN = "playerReachedCabin";
    private const string PLAYER_REACHED_BARN = "playerReachedBarn";
    private const string PLAYER_REACHED_GAS = "playerReachedGas";
    #endregion

    #region PARAMS
    private const string ENEMIES_KILLED = "enemiesKilled";
    private const string ENEMY_SHOTS_FIRED_COUNT = "enemyShotsFiredCount";
    private const string ENEMY_SHOTS_HIT = "enemyShotsHit";
    private const string LEVEL_1_TIME_SPENT = "level1TimeSpent";
    private const string LEVEL_2_TIME_SPENT = "level2TimeSpent";
    private const string ONBOARDING_TIME_SPENT = "onboardingTimeSpent";
    private const string PLAYER_AMMO_CLIP_COUNT = "playerAmmoClipCount";
    private const string PLAYER_BOTTLE_COUNT = "playerBottleCount";
    private const string PLAYER_BOTTLE_THROW_COUNT = "playerBottleThrowCount";
    private const string PLAYER_DEATH_COUNT = "playerDeathCount";
    private const string PLAYER_HEALTH = "playerHealth";
    private const string PLAYER_HEALTH_KIT_COUNT = "playerHealthKitCount";
    private const string PLAYER_PISTOL_AMMO = "playerPistolAmmo";
    private const string PLAYER_PISTOLS_PICKED_UP = "playerPistolsPickedUp";
    private const string PLAYER_SHOTS_FIRED_COUNT = "playerShotsFiredCount";
    private const string PLAYER_SHOTS_HIT = "playerShotsHit";
    private const string PLAYER_TIME_SPENT_AIMING = "playerTimeSpentAiming";
    private const string PLAYER_TIME_SPENT_CROUCHING = "playerTimeSpentCrouching";
    private const string PLAYER_TIME_SPENT_STANDING = "playerTimeSpentStanding";
    private const string PLAYER_TIMES_AIMED = "playerTimesAimed";
    private const string PLAYER_TIMES_CROUCHED = "playerTimesCrouched";
    private const string PLAYER_TIMES_DETECTED = "playerTimesDetected";
    #endregion


    private void SendEvent(string name, Dictionary<string, object> dictionary)
    {
        AnalyticsService.Instance.CustomData(name, dictionary);
    }

    public void SendLevel1Completed(float timeSpent)
    {
        SendEvent(LEVEL_1_COMPLETED, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { LEVEL_1_TIME_SPENT, timeSpent },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_DEATH_COUNT, PlayerParams.playerDeathCount },
                { PLAYER_HEALTH, PlayerParams.health },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
                { PLAYER_TIME_SPENT_AIMING, PlayerParams.playerTimeSpentAiming },
                { PLAYER_TIME_SPENT_CROUCHING, PlayerParams.playerTimeSpentCrouching },
                { PLAYER_TIME_SPENT_STANDING, PlayerParams.playerTimeSpentStanding },
                { PLAYER_TIMES_AIMED, PlayerParams.playerTimesAimed },
                { PLAYER_TIMES_CROUCHED, PlayerParams.playerTimesCrouched },
                { PLAYER_TIMES_DETECTED, PlayerParams.playerTimesDetected }
            });
    }

    public void SendLevel2Completed(float timeSpent)
    {
        SendEvent(LEVEL_2_COMPLETED, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { LEVEL_2_TIME_SPENT, timeSpent },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_DEATH_COUNT, PlayerParams.playerDeathCount },
                { PLAYER_HEALTH, PlayerParams.health },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
                { PLAYER_TIME_SPENT_AIMING, PlayerParams.playerTimeSpentAiming },
                { PLAYER_TIME_SPENT_CROUCHING, PlayerParams.playerTimeSpentCrouching },
                { PLAYER_TIME_SPENT_STANDING, PlayerParams.playerTimeSpentStanding },
                { PLAYER_TIMES_AIMED, PlayerParams.playerTimesAimed },
                { PLAYER_TIMES_CROUCHED, PlayerParams.playerTimesCrouched },
                { PLAYER_TIMES_DETECTED, PlayerParams.playerTimesDetected }
            });
    }

    public void SendOnboardingCompleted(float onboardingTimeSpent)
    {
        SendEvent(ONBOARDING_COMPLETED, new Dictionary<string, object>
            {
                {ONBOARDING_TIME_SPENT, onboardingTimeSpent }
            });
    }

    public void SendLevel1Quit(float timeSpent)
    {
        SendEvent(LEVEL_1_QUIT, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { LEVEL_1_TIME_SPENT, timeSpent },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_DEATH_COUNT, PlayerParams.playerDeathCount },
                { PLAYER_HEALTH, PlayerParams.health },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
                { PLAYER_TIME_SPENT_AIMING, PlayerParams.playerTimeSpentAiming },
                { PLAYER_TIME_SPENT_CROUCHING, PlayerParams.playerTimeSpentCrouching },
                { PLAYER_TIME_SPENT_STANDING, PlayerParams.playerTimeSpentStanding },
                { PLAYER_TIMES_AIMED, PlayerParams.playerTimesAimed },
                { PLAYER_TIMES_CROUCHED, PlayerParams.playerTimesCrouched },
                { PLAYER_TIMES_DETECTED, PlayerParams.playerTimesDetected }
            });
    }

    public void SendLevel2Quit(float timeSpent)
    {
        SendEvent(LEVEL_2_QUIT, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { LEVEL_2_TIME_SPENT, timeSpent },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_DEATH_COUNT, PlayerParams.playerDeathCount },
                { PLAYER_HEALTH, PlayerParams.health },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
                { PLAYER_TIME_SPENT_AIMING, PlayerParams.playerTimeSpentAiming },
                { PLAYER_TIME_SPENT_CROUCHING, PlayerParams.playerTimeSpentCrouching },
                { PLAYER_TIME_SPENT_STANDING, PlayerParams.playerTimeSpentStanding },
                { PLAYER_TIMES_AIMED, PlayerParams.playerTimesAimed },
                { PLAYER_TIMES_CROUCHED, PlayerParams.playerTimesCrouched },
                { PLAYER_TIMES_DETECTED, PlayerParams.playerTimesDetected }
            });
    }

    public void SendPlayerDie()
    {
        SendEvent(PLAYER_DIE, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_DEATH_COUNT, PlayerParams.playerDeathCount },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
                { PLAYER_TIMES_DETECTED, PlayerParams.playerTimesDetected }
            });
    }

    public void SendEnemyDie()
    {
        SendEvent(ENEMY_DIE, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_DEATH_COUNT, PlayerParams.playerDeathCount },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
            });
    }

    public void SendPlayerReachedCabin(float timeSpent)
    {
        SendEvent(PLAYER_REACHED_CABIN, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { LEVEL_2_TIME_SPENT, timeSpent },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_HEALTH, PlayerParams.health },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
                { PLAYER_TIME_SPENT_AIMING, PlayerParams.playerTimeSpentAiming },
                { PLAYER_TIME_SPENT_CROUCHING, PlayerParams.playerTimeSpentCrouching },
                { PLAYER_TIME_SPENT_STANDING, PlayerParams.playerTimeSpentStanding },
                { PLAYER_TIMES_AIMED, PlayerParams.playerTimesAimed },
                { PLAYER_TIMES_CROUCHED, PlayerParams.playerTimesCrouched },
                { PLAYER_TIMES_DETECTED, PlayerParams.playerTimesDetected }
            });
    }

    public void SendPlayerReachedBarn(float timeSpent)
    {
        SendEvent(PLAYER_REACHED_BARN, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { LEVEL_2_TIME_SPENT, timeSpent },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_HEALTH, PlayerParams.health },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
                { PLAYER_TIME_SPENT_AIMING, PlayerParams.playerTimeSpentAiming },
                { PLAYER_TIME_SPENT_CROUCHING, PlayerParams.playerTimeSpentCrouching },
                { PLAYER_TIME_SPENT_STANDING, PlayerParams.playerTimeSpentStanding },
                { PLAYER_TIMES_AIMED, PlayerParams.playerTimesAimed },
                { PLAYER_TIMES_CROUCHED, PlayerParams.playerTimesCrouched },
                { PLAYER_TIMES_DETECTED, PlayerParams.playerTimesDetected }
            });
    }

    public void SendPlayerReachedGas(float timeSpent)
    {
        SendEvent(PLAYER_REACHED_GAS, new Dictionary<string, object>
            {
                { ENEMIES_KILLED, PlayerParams.enemiesKilled },
                { ENEMY_SHOTS_FIRED_COUNT, PlayerParams.enemyShotsFiredCount },
                { ENEMY_SHOTS_HIT, PlayerParams.enemyShotsHit },
                { LEVEL_2_TIME_SPENT, timeSpent },
                { PLAYER_AMMO_CLIP_COUNT, PlayerParams.playerAmmoClipCount },
                { PLAYER_BOTTLE_COUNT,  PlayerParams.playerBottleCount },
                { PLAYER_BOTTLE_THROW_COUNT, PlayerParams.playerBottleThrowCount },
                { PLAYER_HEALTH, PlayerParams.health },
                { PLAYER_HEALTH_KIT_COUNT, PlayerParams.playerHealthKitCount },
                { PLAYER_PISTOL_AMMO, PlayerParams.currentAmmo + PlayerParams.currentClip },
                { PLAYER_PISTOLS_PICKED_UP, PlayerParams.playerPistolsPickedUp },
                { PLAYER_SHOTS_FIRED_COUNT, PlayerParams.playerShotsFiredCount },
                { PLAYER_SHOTS_HIT, PlayerParams.playerShotsHit },
                { PLAYER_TIME_SPENT_AIMING, PlayerParams.playerTimeSpentAiming },
                { PLAYER_TIME_SPENT_CROUCHING, PlayerParams.playerTimeSpentCrouching },
                { PLAYER_TIME_SPENT_STANDING, PlayerParams.playerTimeSpentStanding },
                { PLAYER_TIMES_AIMED, PlayerParams.playerTimesAimed },
                { PLAYER_TIMES_CROUCHED, PlayerParams.playerTimesCrouched },
                { PLAYER_TIMES_DETECTED, PlayerParams.playerTimesDetected }
            });
    }
}

#endif
