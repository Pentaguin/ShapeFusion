using UnityEngine;
using UnityEngine.InputSystem;

public class WalkableCamera : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset inputActions;
    public string actionMapName = "Player";
    public string moveActionName = "Move";
    public string lookActionName = "Look";
    public string jumpActionName = "Jump";

    [Header("References")]
    public Transform cameraPivot;

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public LayerMask draggableLayer;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private CharacterController controller;
    private float pitch = 0f;
    private float verticalVelocity = 0f;
    private bool jumpPressed = false;
    private bool wasHoveringDraggable = false;
    private float rayDistance = 100f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        var map = inputActions.FindActionMap(actionMapName, true);
        moveAction = map.FindAction(moveActionName, true);
        lookAction = map.FindAction(lookActionName, true);
        jumpAction = map.FindAction(jumpActionName, true);

        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();

        jumpAction.performed += _ => jumpPressed = true;

        SetCursorState(true);
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        HandleCursorToggle();
    }

    void HandleMovement()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        move *= moveSpeed;

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (jumpPressed)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpPressed = false;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    void HandleLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * lookSensitivity * Time.deltaTime;

        pitch -= lookInput.y;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        transform.Rotate(Vector3.up * lookInput.x);
    }

    void HandleCursorToggle()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, draggableLayer))
        {
            if (!wasHoveringDraggable)
            {
                SetCursorState(false); // Unlock cursor
                wasHoveringDraggable = true;
            }
        }
        else
        {
            if (wasHoveringDraggable)
            {
                SetCursorState(true); // Lock cursor
                wasHoveringDraggable = false;
            }
        }
    }

    void SetCursorState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
