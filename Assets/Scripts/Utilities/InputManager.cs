using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction interactAction;
    private InputAction pauseAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];
        crouchAction = playerInput.actions["Crouch"];
        interactAction = playerInput.actions["Interact"];
        pauseAction = playerInput.actions["Pause"];

        pauseAction.performed += OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        UIManager.Instance.TogglePauseMenu();
    }

    public Vector2 GetMoveInput() => moveAction.ReadValue<Vector2>();
    public bool IsSprintPressed() => sprintAction.IsPressed();
    public bool IsCrouchPressed() => crouchAction.IsPressed();
    public bool IsInteractPressed() => interactAction.IsPressed();

    private void OnDestroy()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePerformed;
        }
    }
}
