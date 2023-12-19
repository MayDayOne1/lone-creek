public static class PlayerParams
{
    public static bool hasThrowable = false;
    public static bool hasPrimary = false;
    public static bool savedThrowable;
    public static bool savedPrimary;

#if ENABLE_CLOUD_SERVICES_ANALYTICS

    // CONTROLLER
    public static float health = 1f;
    public static float savedHealth;
    public static int enemiesKilled = 0;
    public static int enemyShotsFiredCount = 0;
    public static int enemyShotsHit = 0;
    public static int playerDeathCount = 0;
    public static int playerHealthKitCount = 0;
    public static int playerTimesCrouched = 0;
    public static int playerTimesDetected = 0;
    public static float playerTimeSpentCrouching = 0f;
    public static float playerTimeSpentStanding = 0f;

    // AMMO
    public static int currentAmmo = 0;
    public static int currentClip = 0;
    public static int savedAmmo;
    public static int savedClip;

    // INTERACT
    public static int playerBottleCount = 0;
    public static int playerAmmoClipCount = 0;
    public static int playerPistolsPickedUp = 0;

    // SHOOTING
    public static int playerTimesAimed = 0;
    public static float playerTimeSpentAiming = 0f;

    // THROWABLE
    public static int playerBottleThrowCount = 0;

    // PISTOL
    public static int playerShotsFiredCount = 0;
    public static int playerShotsHit = 0;

#endif
}
