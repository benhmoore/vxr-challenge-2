using UnityEngine;
using UnityEngine.InputSystem; // Make sure to include the new Input System namespace

[RequireComponent(typeof(CharacterController))]
public class NoGravityCharacterController : MonoBehaviour
{
    // Reference to the CharacterController component
    private CharacterController characterController;

    // Movement speed
    public float moveSpeed = 6f;
    
    // Input actions
    public InputAction movementAction;

    private Vector2 movementInput;
    
    void Awake()
    {
        // Get the CharacterController component on this GameObject
        characterController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        // Enable the input action
        movementAction.Enable();

        // Subscribe to the input action events
        movementAction.performed += OnMovementPerformed;
        movementAction.canceled += OnMovementCanceled;
    }

    void OnDisable()
    {
        // Disable the input action
        movementAction.Disable();

        // Unsubscribe from the input action events
        movementAction.performed -= OnMovementPerformed;
        movementAction.canceled -= OnMovementCanceled;
    }

    void Update()
    {
        // Create a movement vector based on the input (moveX and moveZ)
        Vector3 movement = transform.right * movementInput.x + transform.forward * movementInput.y;

        // Call the Move method to actually move the character
        Move(movement);
    }


    // Public method to move without applying gravity
    public void Move(Vector3 motion)
    {
        // Here we explicitly ignore gravity by not modifying the Y-axis
        //motion.y = 0;  // Disable vertical movement caused by gravity

        // Move the character using the CharacterController
        characterController.Move(motion * moveSpeed * Time.deltaTime);
    }

    // Input callback when movement input is performed
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    // Input callback when movement input is canceled
    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }
}
