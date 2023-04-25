using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraMainTransform;
    private Vector2 movement;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
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
        movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new(movement.x, 0, movement.y);
        Vector3 normalizedMove = Vector3.Normalize(move);
        animator.SetFloat("Forward", normalizedMove.x);
        animator.SetFloat("Strafe", normalizedMove.z);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;

        controller.Move(playerSpeed * Time.deltaTime * move);
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
}
/*ALTERNATYWNY KOD
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float jumpHeight = 10f; // nowa zmienna dla skoku

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraMainTransform;
    private Vector2 movement;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
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
        CalculateCharacterRotation();

        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer) // nowa obs³uga skoku
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
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
        movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        controller.Move(playerSpeed * Time.deltaTime * move);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void CalculateCharacterRotation()
    {
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}

*/
