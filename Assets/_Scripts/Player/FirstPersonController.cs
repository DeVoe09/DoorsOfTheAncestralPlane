using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    [Header("Emotion Abilities")]
    public float dashForce = 15f;
    public int maxJumps = 2;

    // Private variables
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    private int jumpsRemaining = 0;
    private bool canDash = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (controller == null)
        {
            Debug.LogError("CharacterController component not found!");
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera not found! Please assign cameraTransform in inspector.");
        }

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (controller == null || cameraTransform == null) return;

        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleEmotionAbilities();
        ApplyGravity();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Get speed multiplier from EmotionManager
        float speedMultiplier = 1f;
        if (EmotionManager.Instance != null)
        {
            speedMultiplier = EmotionManager.Instance.GetCurrentSpeedMultiplier();
        }

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        currentSpeed *= speedMultiplier;

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        // Check if player is grounded (with a small tolerance)
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep player grounded
            jumpsRemaining = maxJumps; // Reset jumps when grounded
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpsRemaining = maxJumps - 1;
                Debug.Log("Jumped! Grounded: " + isGrounded);
            }
            else if (EmotionManager.Instance != null && EmotionManager.Instance.CanDoubleJump() && jumpsRemaining > 0)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpsRemaining--;
                Debug.Log("Double Jump! Jumps remaining: " + jumpsRemaining);
            }
            else
            {
                Debug.Log("Cannot jump. Grounded: " + isGrounded + ", Jumps remaining: " + jumpsRemaining);
            }
        }
    }

    private void HandleEmotionAbilities()
    {
        // Dash ability (Anger emotion)
        if (Input.GetKeyDown(KeyCode.LeftControl) && EmotionManager.Instance != null && EmotionManager.Instance.HasDash())
        {
            Vector3 dashDirection = transform.forward;
            if (Input.GetKey(KeyCode.W)) dashDirection = transform.forward;
            else if (Input.GetKey(KeyCode.S)) dashDirection = -transform.forward;
            else if (Input.GetKey(KeyCode.A)) dashDirection = -transform.right;
            else if (Input.GetKey(KeyCode.D)) dashDirection = transform.right;

            controller.Move(dashDirection * dashForce * Time.deltaTime);
            Debug.Log("DASH!");
        }

        // Slow-mo perception (Calm emotion)
        if (EmotionManager.Instance != null && EmotionManager.Instance.HasSlowMo())
        {
            // Slow-mo is handled by Time.timeScale in EmotionManager or separate script
            // This is just a placeholder for the ability
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Debug.Log("Slow-mo perception activated");
            }
        }
    }

    private void ApplyGravity()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpsRemaining = maxJumps; // Reset jumps when grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Public methods for external control
    public void SetSpeedMultiplier(float multiplier)
    {
        // This is handled by EmotionManager now
        Debug.Log("Speed multiplier set to: " + multiplier);
    }

    public void ResetJumps()
    {
        jumpsRemaining = maxJumps;
    }

    public void UnlockDoubleJump()
    {
        maxJumps = 2;
        Debug.Log("Double jump unlocked!");
    }

    public void UnlockDash()
    {
        canDash = true;
        Debug.Log("Dash unlocked!");
    }
}
