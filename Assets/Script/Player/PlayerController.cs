using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float runSpeed = 4.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float crouchingHeight = 1.0f;
    public Animator animator;
    public float Sensitivity = 1f;

    private CharacterController controller;
    private PlayerShootingManager playerShootingManager;
    private Transform cameraMainTransform;
    private Vector2 movement;
    private Vector3 playerVelocity;

    private float speed;
    private bool groundedPlayer;
    private bool isCrouching = false;

    private float health = 1f;

    public Slider healthSlider;
    public GameObject GameOverScreen;
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerShootingManager = GetComponent<PlayerShootingManager>();
        cameraMainTransform = Camera.main.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameOverScreen.SetActive(false);
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

    private void IsPlayerGrounded()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    private void CrouchToStandLogic()
    {
        if (!isCrouching)
        {
            controller.height = standingHeight;
            controller.center = new Vector3(controller.center.x, 0.9f, controller.center.z);
            speed = runSpeed;
            animator.SetLayerWeight(1, 0);
        }
    }
    private void AimingLogic()
    {
        if (playerShootingManager.IsAimingPistol)
        {
            speed = crouchSpeed;
            animator.SetBool("isAimingPistol", true);
        }
        else
        {
            animator.SetBool("isAimingPistol", false);
            speed = runSpeed;
        }
    }
    private void InputSystemMove()
    {
        movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new(movement.x, 0, movement.y);
        Vector3 normalizedMove = Vector3.Normalize(move);
        animator.SetFloat("Forward", normalizedMove.x);
        animator.SetFloat("Strafe", normalizedMove.z);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;

        controller.Move(speed * Time.deltaTime * move);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void Move()
    {
        CrouchToStandLogic();
        AimingLogic();
        InputSystemMove();
        
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
        isCrouching = !isCrouching;
        if(isCrouching)
        {
            controller.height = crouchingHeight;
            controller.center = new Vector3(controller.center.x, 0.48f, controller.center.z);
            speed = crouchSpeed;
            animator.SetLayerWeight(1, 1);
        }
        
        // Debug.Log("speed:" + speed);
    }
    private void Die()
    {
        Time.timeScale = 0;
        this.gameObject.SetActive(false);
        GameOverScreen.SetActive(true);
    }
    public void PlayerTakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            // Debug.Log("Player is dead");
            health = 0f;
            Die();
        }
        healthSlider.value = health;
    }
}