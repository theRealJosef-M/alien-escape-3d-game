using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseWalkSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float accelerationTime = 0.1f;
    [SerializeField] private float decelerationTime = 0.1f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 15f;
    [SerializeField] private float staminaRegenRate = 10f;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private float crouchDetectionRadius = 8f;

    private CharacterController characterController;
    private Animator animator;
    private PlayerStats playerStats;
    private GameManager gameManager;

    private Vector3 velocity = Vector3.zero;
    private float currentSpeed = 0f;
    private float currentStamina;
    private bool isCrouching = false;
    private bool isSprinting = false;
    private bool isMoving = false;

    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction crouchAction;

    private void OnEnable()
    {
        moveAction?.Enable();
        sprintAction?.Enable();
        crouchAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        sprintAction?.Disable();
        crouchAction?.Disable();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        gameManager = FindObjectOfType<GameManager>();

        currentStamina = maxStamina;

        var inputSystem = GetComponent<PlayerInput>();
        moveAction = inputSystem.actions["Move"];
        sprintAction = inputSystem.actions["Sprint"];
        crouchAction = inputSystem.actions["Crouch"];
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleStamina();
        UpdateAnimator();
    }

    private void HandleInput()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        isMoving = moveInput.magnitude > 0.1f;

        if (crouchAction.triggered)
        {
            isCrouching = !isCrouching;
        }

        if (sprintAction.IsPressed() && currentStamina > 0 && isMoving && !isCrouching)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        float targetSpeed = 0f;

        if (isMoving)
        {
            if (isCrouching)
            {
                targetSpeed = crouchSpeed;
            }
            else if (isSprinting)
            {
                targetSpeed = sprintSpeed;
            }
            else
            {
                targetSpeed = baseWalkSpeed;
            }
        }

        // Smooth speed transition
        float accelerationRate = targetSpeed > currentSpeed ? accelerationTime : decelerationTime;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / accelerationRate);

        // Apply movement
        if (characterController.isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= 9.81f * Time.deltaTime;
        }

        Vector3 motion = transform.TransformDirection(moveDirection) * currentSpeed;
        motion.y = velocity.y;
        characterController.Move(motion * Time.deltaTime);

        // Rotate player towards movement direction
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void HandleStamina()
    {
        if (isSprinting && isMoving)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(0, currentStamina);
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(maxStamina, currentStamina);
        }

        playerStats.UpdateStamina(currentStamina / maxStamina);
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("IsGrounded", characterController.isGrounded);
    }

    public float GetDetectionRadius()
    {
        return isCrouching ? crouchDetectionRadius : detectionRadius;
    }

    public bool IsCrouching() => isCrouching;
    public bool IsSprinting() => isSprinting;
    public float GetCurrentStamina() => currentStamina;
    public float GetMaxStamina() => maxStamina;
}
