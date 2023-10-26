using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float crouchingHeight = 1.0f;
    private PlayerAnimManager animManager;
    private PlayerCamManager camManager;
    private float speed;
    public float runSpeed = 4.0f;
    public float Sensitivity = 1f;

    private const string IS_CROUCHING = "isCrouching";
    private const string IS_AIMING_PISTOL = "isAimingPistol";

    private CharacterController controller;
    private PlayerShootingManager playerShootingManager;
    private PlayerInteract playerInteract;
    private Transform cameraMainTransform;
    private Vector2 movement;
    private Vector3 playerVelocity;

    private bool groundedPlayer;
    public bool IsCrouching;

    private float health = 1f;
    public bool isShowingPauseMenu = false;
    public Image bloodOverlay;

    public Slider healthSlider;
    public GameObject GameOverScreen;

    [SerializeField] private GameObject PauseMenu;
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
        bloodOverlay.color = new Color(255f, 255f, 255f, 0f);

        Time.timeScale = 1;
        camManager.EnableAll(true);
        this.gameObject.SetActive(true);
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
        animManager.SetFloat("Forward", normalizedMove.x);
        animManager.SetFloat("Strafe", normalizedMove.z);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;

        SpeedUpdater();

        controller.Move(speed * Time.deltaTime * move);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void SpeedUpdater()
    {
        if(IsCrouching || playerShootingManager.IsAimingPistol || playerShootingManager.IsAimingThrowable) 
        {
            speed = crouchSpeed;
        } else
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
            if(playerInteract.Pistol.activeSelf)
            {
                animManager.SetPistol(false);
                animManager.SetPistolCrouch(true);
            }
            
            animManager.SetCrouch(true);
            
        } else
        {
            controller.height = standingHeight;
            controller.center = new Vector3(controller.center.x, 0.9f, controller.center.z);
            camManager.ActivateNormal();
            if (playerInteract.Pistol.activeSelf)
            {
                animManager.SetPistol(true);
                animManager.SetPistolCrouch(false);
            }
            
            animManager.SetCrouch(false);
        }
    }
    private void Die()
    {
        Time.timeScale = 0;
        camManager.EnableAll(false);
        this.gameObject.SetActive(false);
        GameOverScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void PlayerTakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            health = 0f;
            // Die();
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