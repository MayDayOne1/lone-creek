using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private float runSpeed = 4.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float crouchingHeight = 1.0f;

    private PlayerShootingManager playerShootingManager;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraMainTransform;
    private Vector2 movement;
    private float speed;

    private bool isCrouching = false;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerShootingManager = GetComponent<PlayerShootingManager>();
        cameraMainTransform = Camera.main.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        movementControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
    }

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

    private void Move()
    {
        if (!isCrouching)
        {
            controller.height = standingHeight;
            controller.center = new Vector3(controller.center.x, 0.9f, controller.center.z);
            speed = runSpeed;
            animator.SetLayerWeight(1, 0);
        }
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

    private void CalculateCharacterRotation()
    {
        if (movement != Vector2.zero)
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
}

