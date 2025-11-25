using UnityEngine;

/// <summary>
/// Third-person character controller for Mixamo animations.
/// Designed for in-place animations with CharacterController movement.
/// </summary>
public class MixamoCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Normal walking speed")]
    public float walkSpeed = 3f;
    
    [Tooltip("Sprinting speed when holding Shift")]
    public float sprintSpeed = 6f;
    
    [Tooltip("Crouching movement speed")]
    public float crouchSpeed = 1.5f;
    
    [Tooltip("How fast the character rotates to face movement direction")]
    public float rotationSpeed = 10f;
    
    [Tooltip("Gravity force applied to character")]
    public float gravity = 20f;

    [Header("References")]
    [Tooltip("The main camera (will auto-find if not assigned)")]
    public Transform cameraTransform;

    // Component references
    private CharacterController characterController;
    private Animator animator;
    
    // Movement variables
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private float verticalVelocity = 0f;
    
    // State tracking
    private bool isCrouching = false;
    private bool isSprinting = false;
    private bool isMoving = false;

    void Start()
    {
        // Get required components
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        // Auto-find camera if not assigned
        if (cameraTransform == null)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
            else
                Debug.LogError("No camera found! Please assign a camera or tag your main camera as 'MainCamera'");
        }
        
        // Verify components exist
        if (characterController == null)
            Debug.LogError("CharacterController component missing! Add one to " + gameObject.name);
        
        if (animator == null)
            Debug.LogError("Animator component missing! Add one to " + gameObject.name);
    }

    void Update()
    {
        if (characterController == null || animator == null)
            return;

        HandleInput();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (characterController == null)
            return;

        ApplyMovement();
    }

    /// <summary>
    /// Handle all player input
    /// </summary>
    void HandleInput()
    {
        // Get movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        // Calculate movement direction relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        // Flatten to horizontal plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        // Combine input with camera direction
        moveDirection = (forward * vertical + right * horizontal).normalized;
        
        // Check if we're moving
        isMoving = moveDirection.magnitude > 0.1f;
        
        // Handle crouch toggle
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }
        
        // Handle sprint (only when moving and not crouching)
        isSprinting = isMoving && Input.GetKey(KeyCode.LeftShift) && !isCrouching;
    }

    /// <summary>
    /// Apply movement to the character
    /// </summary>
    void ApplyMovement()
    {
        // Determine current speed based on state
        float currentSpeed = walkSpeed;
        if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (isSprinting)
            currentSpeed = sprintSpeed;
        
        // Calculate horizontal movement
        Vector3 horizontalMove = moveDirection * currentSpeed;
        
        // Apply gravity
        if (characterController.isGrounded)
        {
            verticalVelocity = -2f; // Small downward force to keep grounded
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        
        // Combine horizontal and vertical movement
        velocity = horizontalMove + Vector3.up * verticalVelocity;
        
        // Move the character
        characterController.Move(velocity * Time.deltaTime);
        
        // Rotate character to face movement direction
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Update animator parameters based on current state
    /// </summary>
    void UpdateAnimations()
    {
        // Update all animator parameters
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isSprinting", isSprinting);
        animator.SetBool("isGrounded", characterController.isGrounded);
        
        // Optional: set speed as a float for blend trees
        float speed = 0f;
        if (isMoving)
        {
            if (isSprinting)
                speed = 2f;
            else if (isCrouching)
                speed = 0.5f;
            else
                speed = 1f;
        }
        animator.SetFloat("speed", speed);
    }
}