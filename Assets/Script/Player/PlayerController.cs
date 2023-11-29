using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float blendSpaceDampTime = .1f;
    public float runSpeed;
    private float speed;
    private bool isGrounded;
    private Vector2 movement;
    private Vector3 playerVelocity;

    [Header("CROUCH")]
    [SerializeField] private float crouchingHeight = 1.0f;
    public float crouchSpeed;
    public bool IsCrouching;

    [Header("CAMERA")]
    private PlayerCamManager camManager;
    private Transform cameraMainTransform;

    [Header("UI")]
    [SerializeField] private GameObject PauseMenu;
    public bool isShowingPauseMenu = false;
    public Image bloodOverlay;
    public Slider healthSlider;
    public CanvasGroup GameOverScreen;
    public CanvasGroup gameOverBlackout;
    public GameObject HUD;

    [Header("HEALTH")]
    public static float health = 1f;
    public static float savedHealth;
    private Rigidbody[] childrenRB;

    [Header("COROUTINES")]
    public IEnumerator level1coroutine;
    public IEnumerator level2coroutine;

    private CharacterController controller;
    private PlayerInput playerInput;
    private PlayerAnimManager animManager;
    private PlayerShootingManager playerShootingManager;
    private PlayerInteract playerInteract;
    private PlayerAudioManager audioManager;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    public static int enemiesKilled = 0;
    public static int enemyShotsFiredCount = 0;
    public static int enemyShotsHit = 0;
    public static int playerDeathCount = 0;
    public static int playerHealthKitCount = 0;
    public float onboardingTimeSpent = 0f;
    public float level1TimeSpent = 0f;
    public float level2TimeSpent = 0f;
    public static int playerTimesCrouched = 0;
    public static float playerTimeSpentCrouching = 0f;
    public static float playerTimeSpentStanding = 0f;

#endif
  
    private void Start()
    {
        LeanTween.cancelAll();
        DOTween.ClearCachedTweens();

        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        playerShootingManager = GetComponent<PlayerShootingManager>();
        playerInteract = GetComponent<PlayerInteract>();
        animManager = GetComponent<PlayerAnimManager>();
        camManager = GetComponent<PlayerCamManager>();
        audioManager = GetComponent<PlayerAudioManager>();
        cameraMainTransform = Camera.main.transform;

        playerInput.ActivateInput();
        camManager.ActivateNormal();
        animManager.AnimatorSetter(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ShowGameOverScreen(false);

        IsCrouching = false;
        StartCoroutine(CountStandingTime());
        animManager.DisableAllLayers();

        PauseMenu.SetActive(false);
        HUD.SetActive(true);
        bloodOverlay.color = new Color(255f, 255f, 255f, 0f);

        Time.timeScale = 1;
        camManager.EnableAll(true);
        this.gameObject.SetActive(true);
        Checkpoint();
        LoadFromCheckpoint();

        childrenRB = this.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childrenRB)
        {
            rb.isKinematic = true;
        }
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        level1coroutine = Level1Timer();
        level2coroutine = Level2Timer();
        if (SceneManager.GetActiveScene().name == "SceneTunnel")
        {
            StartCoroutine(level1coroutine);
        } else if(SceneManager.GetActiveScene().name == "SceneDesert")
        {
            StartCoroutine(level2coroutine);
        }
#endif
    }
    #region MovementControlEnableDisable
    private void OnEnable()
    {
        movementControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
    }
    #endregion

    void Update()
    {
        IsPlayerGrounded();
        Move();
    }

    private void FixedUpdate()
    {
        CalculateCharacterRotation();  
    }

    public void ResetAnalyticsData()
    {
        enemiesKilled = 0;
        enemyShotsFiredCount = 0;
        enemyShotsHit = 0;
        playerDeathCount = 0;
        playerHealthKitCount = 0;
        onboardingTimeSpent = 0f;
        level1TimeSpent = 0f;
        level2TimeSpent = 0f;
        playerTimesCrouched = 0;
        playerTimeSpentCrouching = 0f;
        playerTimeSpentStanding = 0f;

        // ammo reset in LoadFromCheckpoint(), no reason to do it twice
        PlayerShootingManager.playerBottleThrowCount = 0;
        PlayerShootingManager.playerShotsFiredCount = 0;
        PlayerShootingManager.playerShotsHit = 0;
        PlayerShootingManager.playerTimesAimed = 0;
        PlayerShootingManager.playerTimeSpentAiming = 0;

        PlayerInteract.playerAmmoClipCount = 0;
        PlayerInteract.playerBottleCount = 0;
        PlayerInteract.playerPistolsPickedUp = 0;
}
    private IEnumerator Level1Timer()
    {
        while(SceneManager.GetActiveScene().buildIndex == 1)
        {
            level1TimeSpent += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator Level2Timer()
    {
        while(SceneManager.GetActiveScene().buildIndex == 2)
        {
            level2TimeSpent += Time.deltaTime;
            yield return null;
        }
    }
    private void Checkpoint()
    {
        savedHealth = health;
        healthSlider.value = savedHealth;
        PlayerInteract.savedThrowable = PlayerInteract.hasThrowable;
        PlayerInteract.savedPrimary = PlayerInteract.hasPrimary;
        PlayerAmmoManager.savedAmmo = PlayerAmmoManager.currentAmmo;
        PlayerAmmoManager.savedClip = PlayerAmmoManager.currentClip;
    }
    private void LoadFromCheckpoint()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            health = savedHealth;
            PlayerInteract.hasThrowable = PlayerInteract.savedThrowable;
            PlayerInteract.hasPrimary = PlayerInteract.savedPrimary;
            healthSlider.value = health;
            PlayerAmmoManager.currentAmmo = PlayerAmmoManager.savedAmmo;
            PlayerAmmoManager.currentClip = PlayerAmmoManager.savedClip;
        }
        else
        {
            health = 1;
            healthSlider.value = health;
            PlayerInteract.hasThrowable = false;
            PlayerInteract.hasPrimary = false;
            PlayerAmmoManager.currentAmmo = 0;
            PlayerAmmoManager.currentClip = 0;
        }
    }
    public float GetHealth() => health;
    public void SetSpeed(float otherSpeed)
    {
        speed = otherSpeed;
    }
    private void BloodOverlayAnim()
    {
        bloodOverlay.DOFade(60f, 1f);
        bloodOverlay.DOFade(0f, 1f);
    }
    private void IsPlayerGrounded()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }
    private void Move()
    {
        InputSystemMove();
    }
    private void InputSystemMove()
    {
        movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new(movement.x, 0, movement.y);
        Vector3 normalizedMove = Vector3.Normalize(move);

        animManager.SetFloat("Forward", normalizedMove.x, blendSpaceDampTime, Time.deltaTime);
        animManager.SetFloat("Strafe", normalizedMove.z, blendSpaceDampTime, Time.deltaTime);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;

        UpdateSpeed();

        // Debug.Log(controller.velocity.magnitude);

        controller.Move(speed * Time.deltaTime * move);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void UpdateSpeed()
    {
        if(IsCrouching || playerShootingManager.IsAimingPistol || playerShootingManager.IsAimingThrowable) 
        {
            speed = crouchSpeed;
        }
        else
        {
            speed = runSpeed;
        }
    }
    public void CalculateCharacterRotation()
    {
        if (movement != Vector2.zero || playerShootingManager.IsAimingThrowable || playerShootingManager.IsAimingPistol)
        {
            float yawCamera = cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, yawCamera, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
    private IEnumerator CountStandingTime()
    {
        while(!IsCrouching)
        {
            playerTimeSpentStanding += Time.deltaTime;
            yield return null;
        }
        
    }    
    private IEnumerator CountCrouchTime()
    {
        while(IsCrouching)
        {
            playerTimeSpentCrouching += Time.deltaTime;
            yield return null;
        }
    }
    public void Crouch()
    {
        IsCrouching = !IsCrouching;
        if(IsCrouching)
        {
            controller.height = crouchingHeight;
            controller.center = new Vector3(controller.center.x, 0.48f, controller.center.z);
            if(playerShootingManager.IsAimingPistol ||
                playerShootingManager.IsAimingThrowable)
            {
                camManager.ActivateCrouchAim();
            }
            else
            {
                camManager.ActivateCrouch();
            }
            animManager.SetCrouch(true, playerInteract.Pistol.activeSelf);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            playerTimesCrouched++;
            StopCoroutine(CountStandingTime());
            StartCoroutine(CountCrouchTime());
#endif
        }
        else
        {
            controller.height = standingHeight;
            controller.center = new Vector3(controller.center.x, 0.9f, controller.center.z);
            if(playerShootingManager.IsAimingPistol ||
                playerShootingManager.IsAimingThrowable)
            {
                camManager.ActivateAim();
            }
            else
            {
                camManager.ActivateNormal();
            }
            animManager.SetCrouch(false, playerInteract.Pistol.activeSelf);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            StopCoroutine(CountCrouchTime());
            StartCoroutine(CountStandingTime());
#endif
        }
    }
    private void SmoothTimeScaleSetter(float timeValue)
    {
        LeanTween.value(gameObject, 1f, timeValue, .4f)
        .setEaseOutQuart()
        .setOnUpdate((timeValue) =>
            {
                Time.fixedDeltaTime *= timeValue;
                Time.timeScale = timeValue;
            });
    }
    private void ShowGameOverScreen(bool show)
    {
        if(show)
        {
            GameOverScreen.DOFade(1f, .4f);
            GameOverScreen.interactable = true;
            GameOverScreen.blocksRaycasts = true;
        }
        else
        {
            gameOverBlackout.DOFade(0f, 0f);
            GameOverScreen.DOFade(0f, 0f);
            GameOverScreen.interactable = false;
            GameOverScreen.blocksRaycasts = false;
        }
    }
    private void ScreenBlackout()
    {
        gameOverBlackout.DOFade(1f, 2f);
    }
    private IEnumerator Die()
    {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        playerDeathCount++;
        AnalyticsService.Instance.CustomData("playerDie", new Dictionary<string, object>()
            {
                { "playerHealthKitCount", playerHealthKitCount },
                { "playerDeathCount", playerDeathCount },
                { "playerPistolAmmo", PlayerAmmoManager.currentAmmo + PlayerAmmoManager.currentClip },
                { "playerAmmoClipCount", PlayerInteract.playerAmmoClipCount },
                { "playerBottleCount",  PlayerInteract.playerBottleCount },
                { "playerBottleThrowCount", PlayerShootingManager.playerBottleThrowCount },
                { "playerShotsFiredCount", PlayerShootingManager.playerShotsFiredCount },
                { "enemiesKilled", enemiesKilled },
                { "enemyShotsFiredCount", enemyShotsFiredCount },
                { "enemyShotsHit", enemyShotsHit },
                { "playerPistolsPickedUp", PlayerInteract.playerPistolsPickedUp },
                { "playerShotsHit", PlayerShootingManager.playerShotsHit }
            });
#endif
        SmoothTimeScaleSetter(.5f);
        playerInput.DeactivateInput();
        camManager.EnableAll(false);
        HUD.SetActive(false);
        animManager.AnimatorSetter(false);
        foreach (Rigidbody r in childrenRB)
        {
            r.isKinematic = false;
        }
        yield return new WaitForSeconds(.5f);
        ScreenBlackout();
        yield return new WaitForSeconds(2.5f);
        audioManager.PlayGameOverSound();
        ShowGameOverScreen(true);
        yield return new WaitForSeconds(.4f);

        Time.timeScale = 0f;
        Time.fixedDeltaTime = .02f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void PlayerTakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            health = 0f;
            StopAllCoroutines();
            StartCoroutine(Die());
        }
        healthSlider.DOValue(health, .2f, false);
        BloodOverlayAnim();
    }
    public void PlayerRestoreHealth(float healthAmount)
    {
        if (health + healthAmount > 1f) health = 1f;
        else health += healthAmount;
        healthSlider.DOValue(health, .5f, false);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        playerHealthKitCount++;
#endif
    }
    public void TogglePauseMenu()
    {
        if(!isShowingPauseMenu)
        {
            PauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            camManager.EnableAll(false);
            
        } else
        {
            PauseMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            camManager.EnableAll(true);
        }
        isShowingPauseMenu = !isShowingPauseMenu;
    }
    public void VictorySetup()
    {
        StopCoroutine(level2coroutine);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        camManager.EnableAll(false);
    }
}