using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float crouchingHeight = 1.0f;
    [SerializeField] private float blendSpaceDampTime = .1f;
    private PlayerAnimManager animManager;
    private PlayerCamManager camManager;
    private float speed;
    public float runSpeed;
    public float Sensitivity = 1f;

    private const string IS_AIMING_PISTOL = "isAimingPistol";

    private CharacterController controller;
    private PlayerShootingManager playerShootingManager;
    private PlayerInteract playerInteract;
    private Transform cameraMainTransform;
    private Vector2 movement;
    private Vector3 playerVelocity;

    private bool groundedPlayer;
    public bool IsCrouching;

    private static float health = 1f;
    public bool isShowingPauseMenu = false;
    public Image bloodOverlay;

    public Slider healthSlider;
    public GameObject GameOverScreen;
    public GameObject HUD;

    [SerializeField] private GameObject PauseMenu;

    private Rigidbody[] childrenRB;

    public static float savedHealth;

    private void Checkpoint()
    {
        savedHealth = health;
        PlayerInteract.savedThrowable = PlayerInteract.hasThrowable;
        PlayerInteract.savedPrimary = PlayerInteract.hasPrimary;
        healthSlider.value = savedHealth;
        PlayerAmmoManager.savedAmmo = PlayerAmmoManager.currentAmmo;
        PlayerAmmoManager.savedClip = PlayerAmmoManager.currentClip;
    }

    private void LoadFromCheckpoint()
    {
        health = savedHealth;
        PlayerInteract.hasThrowable = PlayerInteract.savedThrowable;
        PlayerInteract.hasPrimary = PlayerInteract.savedPrimary;
        healthSlider.value = health;
        PlayerAmmoManager.currentAmmo = PlayerAmmoManager.savedAmmo;
        PlayerAmmoManager.currentClip = PlayerAmmoManager.savedClip;
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerShootingManager = GetComponent<PlayerShootingManager>();
        playerInteract = GetComponent<PlayerInteract>();
        animManager = GetComponent<PlayerAnimManager>();
        camManager = GetComponent<PlayerCamManager>();
        cameraMainTransform = Camera.main.transform;

        camManager.ActivateNormal();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameOverScreen.SetActive(false);
        IsCrouching = false;
        animManager.DisableAllLayers();

        PauseMenu.SetActive(false);
        HUD.SetActive(true);
        bloodOverlay.color = new Color(255f, 255f, 255f, 0f);

        Time.timeScale = 1;
        camManager.EnableAll(true);
        this.gameObject.SetActive(true);

        Checkpoint();

        childrenRB = this.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childrenRB)
        {
            rb.isKinematic = true;
        }

        if(SceneManager.GetActiveScene().name == "SceneDesert")
        {
            Analytics.CustomEvent("level1Completed");
        }

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
        playerShootingManager.Aim();
    }

    private void FixedUpdate()
    {
        CalculateCharacterRotation();  
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
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }
    
    private void Move()
    {
        AimingLogic();
        InputSystemMove();
        
    }
    private void AimingLogic()
    {
        if (playerShootingManager.IsAimingPistol)
        {
            speed = crouchSpeed;
            animManager.SetBool(IS_AIMING_PISTOL, true);
            if (IsCrouching) camManager.ActivateCrouchAim();
            else camManager.ActivateAim();
        }
        else
        {
            speed = runSpeed;
            animManager.SetBool(IS_AIMING_PISTOL, false);
            if (IsCrouching) camManager.ActivateCrouch();
            else camManager.ActivateNormal();
        }
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
    public void Crouch()
    {
        IsCrouching = !IsCrouching;
        if(IsCrouching)
        {
            controller.height = crouchingHeight;
            controller.center = new Vector3(controller.center.x, 0.48f, controller.center.z);
            camManager.ActivateCrouch();
            animManager.SetCrouch(true, playerInteract.Pistol.activeSelf);
        }
        else
        {
            controller.height = standingHeight;
            controller.center = new Vector3(controller.center.x, 0.9f, controller.center.z);
            camManager.ActivateNormal();
            animManager.SetCrouch(false, playerInteract.Pistol.activeSelf);
        }
    }
    private void Die()
    {
        foreach (Rigidbody r in childrenRB)
        {
            r.isKinematic = false;
        }

        LoadFromCheckpoint();

        Time.timeScale = 0;
        camManager.EnableAll(false);
        this.gameObject.SetActive(false);
        GameOverScreen.SetActive(true);
        HUD.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void PlayerTakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            health = 0f;
            Die();
        }
        healthSlider.DOValue(health, .2f, false);
        BloodOverlayAnim();
    }
    public void PlayerRestoreHealth(float healthAmount)
    {
        if (health + healthAmount > 1f) health = 1f;
        else health += healthAmount;
        healthSlider.DOValue(health, .5f, false);
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        camManager.EnableAll(false);
    }
}