using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MEC;
using Zenject;

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
    [SerializeField] private float blendSpaceDampTime = .1f;
    public float runSpeed;
    private float speed;
    private float standingHeight = 1.62f;
    private bool isGrounded;
    private Vector2 movement;
    private Vector3 playerVelocity;

    [Header("CROUCH")]
    public float crouchSpeed;
    public bool IsCrouching;
    private float crouchingHeight = .8f;

    [Header("CAMERA")]
    [Inject] private PlayerCamManager camManager;
    private Transform cameraMainTransform;

    [Header("UI")]
    public bool isShowingPauseMenu = false;
    public Image bloodOverlay;
    public Slider healthSlider;
    public CanvasGroup GameOverScreen;
    public CanvasGroup gameOverBlackout;
    public GameObject HUD;
    [SerializeField] private GameObject PauseMenu;

    private Rigidbody[] childrenRB;

    [Header("COROUTINES")]
    public IEnumerator<float> level1coroutine;
    public IEnumerator<float> level2coroutine;

    private CharacterController controller;
    private PlayerInput playerInput;
    private PlayerAnimManager animManager;
    private PlayerShootingManager shootingManager;
    private PlayerAudioManager audioManager;
    public float Speed
    {
        private get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    [Inject] AnalyticsManager analyticsManager;
#endif

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    [HideInInspector] public float onboardingTimeSpent = 0f;
    [HideInInspector] public float level1TimeSpent = 0f;
    [HideInInspector] public float level2TimeSpent = 0f;

#endif
  
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        shootingManager = GetComponent<PlayerShootingManager>();
        animManager = GetComponent<PlayerAnimManager>();
        audioManager = GetComponent<PlayerAudioManager>();
        cameraMainTransform = Camera.main.transform;
        childrenRB = this.GetComponentsInChildren<Rigidbody>();

        ClearTweens();
        AnimSetup();
        CamSetup();
        UISetup();
        GameSetup();
        ActivateRagdoll(false);

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        Timing.RunCoroutine(CountStandingTime());
        level1coroutine = Level1Timer();
        level2coroutine = Level2Timer();
        if (SceneManager.GetActiveScene().name == "SceneTunnel")
        {
            Timing.RunCoroutine(level1coroutine);
        } else if(SceneManager.GetActiveScene().name == "SceneDesert")
        {
            Timing.RunCoroutine(level2coroutine);
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

    #region GAME SETUP
    private void ClearTweens()
    {
        LeanTween.cancelAll();
        DOTween.ClearCachedTweens();
    }
    private void CamSetup()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void AnimSetup()
    {
        animManager.SetAnimator(true);
        animManager.DisableAllLayers();
    }
    private void UISetup()
    {
        ShowGameOverScreen(false);
        PauseMenu.SetActive(false);
        HUD.SetActive(true);
        bloodOverlay.color = new Color(255f, 255f, 255f, 0f);
    }
    private void GameSetup()
    {
        playerInput.ActivateInput();
        IsCrouching = false;
        Speed = runSpeed;

        Time.timeScale = 1;
        Time.fixedDeltaTime = .02f;
        this.gameObject.SetActive(true);
        Checkpoint();
        LoadFromCheckpoint();
    }
    private void ActivateRagdoll(bool active)
    {
        foreach (Rigidbody rb in childrenRB)
        {
            rb.isKinematic = !active;
        }
    }
    #endregion

    #region ANALYTICS
    public void ResetAnalyticsData()
    {
        PlayerParams.enemiesKilled = 0;
        PlayerParams.enemyShotsFiredCount = 0;
        PlayerParams.enemyShotsHit = 0;
        PlayerParams.playerDeathCount = 0;
        PlayerParams.playerHealthKitCount = 0;
        PlayerParams.playerTimesCrouched = 0;
        PlayerParams.playerTimesDetected = 0;
        PlayerParams.playerTimeSpentCrouching = 0f;
        PlayerParams.playerTimeSpentStanding = 0f;

        onboardingTimeSpent = 0f;
        level1TimeSpent = 0f;
        level2TimeSpent = 0f;

        // ammo reset in LoadFromCheckpoint(), no reason to do it twice
        PlayerParams.playerBottleThrowCount = 0;
        PlayerParams.playerShotsFiredCount = 0;
        PlayerParams.playerShotsHit = 0;
        PlayerParams.playerTimesAimed = 0;
        PlayerParams.playerTimeSpentAiming = 0;

        PlayerParams.playerAmmoClipCount = 0;
        PlayerParams.playerBottleCount = 0;
        PlayerParams.playerPistolsPickedUp = 0;
}
    private IEnumerator<float> Level1Timer()
    {
        while(SceneManager.GetActiveScene().buildIndex == 1)
        {
            level1TimeSpent += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }
    private IEnumerator<float> Level2Timer()
    {
        while(SceneManager.GetActiveScene().buildIndex == 2)
        {
            level2TimeSpent += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }
    private void Checkpoint()
    {
        PlayerParams.savedHealth = PlayerParams.health;
        healthSlider.value = PlayerParams.savedHealth;
        PlayerParams.savedThrowable = PlayerParams.hasThrowable;
        PlayerParams.savedPrimary = PlayerParams.hasPrimary;
        PlayerParams.savedAmmo = PlayerParams.currentAmmo;
        PlayerParams.savedClip = PlayerParams.currentClip;
    }
    private void LoadFromCheckpoint()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            PlayerParams.health = PlayerParams.savedHealth;
            PlayerParams.hasThrowable = PlayerParams.savedThrowable;
            PlayerParams.hasPrimary = PlayerParams.savedPrimary;
            healthSlider.value = PlayerParams.health;
            PlayerParams.currentAmmo = PlayerParams.savedAmmo;
            PlayerParams.currentClip = PlayerParams.savedClip;
        }
        else
        {
            PlayerParams.health = 1;
            healthSlider.value = PlayerParams.health;
            PlayerParams.hasThrowable = false;
            PlayerParams.hasPrimary = false;
            PlayerParams.currentAmmo = 0;
            PlayerParams.currentClip = 0;
        }
    }
    private IEnumerator<float> CountStandingTime()
    {
        while (!IsCrouching)
        {
            PlayerParams.playerTimeSpentStanding += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }
    private IEnumerator<float> CountCrouchTime()
    {
        while (IsCrouching)
        {
            PlayerParams.playerTimeSpentCrouching += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }
    #endregion

    #region MOVEMENT
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
        movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new(movement.x, 0, movement.y);
        Vector3 normalizedMove = Vector3.Normalize(move);

        animManager.SetFloat("Forward", normalizedMove.x, blendSpaceDampTime, Time.deltaTime);
        animManager.SetFloat("Strafe", normalizedMove.z, blendSpaceDampTime, Time.deltaTime);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;

       UpdateSpeed();

        controller.Move(speed * Time.deltaTime * move);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void UpdateSpeed()
    {
        if(IsCrouching || shootingManager.isAiming) 
        {
            Speed = crouchSpeed;
        }
        else
        {
            Speed = runSpeed;
        }
    }
    public void CalculateCharacterRotation()
    {
        if (movement != Vector2.zero || shootingManager.isAiming)
        {
            float yawCamera = cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, yawCamera, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
    #endregion

    #region CROUCH

    public void Crouch()
    {
        IsCrouching = !IsCrouching;
        if(IsCrouching)
        {
            CrouchControllerSetup();
            CrouchCamSetup();

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            PlayerParams.playerTimesCrouched++;
            StopCoroutine(CountStandingTime());
            Timing.RunCoroutine(CountCrouchTime());
#endif

        }
        else
        {
            StandControllerSetup();
            StandCamSetup();

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            StopCoroutine(CountCrouchTime());
            Timing.RunCoroutine(CountStandingTime());
#endif

        }
    }
    private void CrouchControllerSetup()
    {
        controller.height = crouchingHeight;
        controller.center = new Vector3(controller.center.x, 0.48f, controller.center.z);
        animManager.SetCrouch(true);
    }
    private void CrouchCamSetup()
    {
        if (shootingManager.isAiming)
        {
            camManager.ActivateCrouchAim();
        }
        else
        {
            camManager.ActivateCrouch();
        }
    }
    private void StandControllerSetup()
    {
        controller.height = standingHeight;
        controller.center = new Vector3(controller.center.x, 0.9f, controller.center.z);
        animManager.SetCrouch(false);
    }
    private void StandCamSetup()
    {
        if (shootingManager.isAiming)
        {
            camManager.ActivateAim();
        }
        else
        {
            camManager.ActivateNormal();
        }
    }
    #endregion

    #region GAME OVER
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
    private void Die()
    {

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        PlayerParams.playerDeathCount++;
        analyticsManager.SendPlayerDie();
#endif

        DeathSetup();
        Timing.RunCoroutine(DeathSequence());
        NavigateDeathScreen();
    }
    private void DeathSetup()
    {
        playerInput.DeactivateInput();
        camManager.EnableAll(false);
        HUD.SetActive(false);
        animManager.SetAnimator(false);
        ActivateRagdoll(true);
    }
    private IEnumerator<float> DeathSequence()
    {
        SmoothTimeScaleSetter(.5f);
        yield return Timing.WaitForSeconds(.5f);
        ScreenBlackout();
        yield return Timing.WaitForSeconds(2.5f);
        audioManager.PlayGameOverSound();
        ShowGameOverScreen(true);
        yield return Timing.WaitForSeconds(.4f);
    }
    private void NavigateDeathScreen()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = .02f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void VictorySetup()
    {
        StopCoroutine(level2coroutine);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        camManager.EnableAll(false);
    }
    #endregion

    #region HEALTH & DAMAGE
    public void PlayerTakeDamage(float damage)
    {
        camManager.DamageCamShake();
        PlayerParams.health -= damage;
        if(PlayerParams.health <= 0f)
        {
            PlayerParams.health = 0f;
            StopAllCoroutines();
            Die();
        }
        healthSlider.DOValue(PlayerParams.health, .2f, false);
        BloodOverlayAnim();
        audioManager.PlayDamageSound();
    }
    public void PlayerRestoreHealth(float healthAmount)
    {
        if (PlayerParams.health + healthAmount > 1f)
        {
            PlayerParams.health = 1f;
        }
        else
        {
            PlayerParams.health += healthAmount;
        }
        healthSlider.DOValue(PlayerParams.health, .5f, false);

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        PlayerParams.playerHealthKitCount++;
#endif

    }
    #endregion

    #region UI & UX
    public void TogglePauseMenu()
    {
        if (!isShowingPauseMenu)
        {
            PauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            camManager.EnableAll(false);
            playerInput.DeactivateInput();
        }
        else
        {
            PauseMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            camManager.EnableAll(true);
            playerInput.ActivateInput();
        }
        isShowingPauseMenu = !isShowingPauseMenu;
    }
    private void BloodOverlayAnim()
    {
        bloodOverlay.DOFade(60f, 1f);
        bloodOverlay.DOFade(0f, 1f);
    }
    #endregion

}