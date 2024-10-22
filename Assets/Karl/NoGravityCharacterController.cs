using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NoGravityCharacterController : MonoBehaviour
{
    // Reference to the CharacterController component
    private CharacterController characterController;

    // Movement speed
    public float moveSpeed = 6f;

    void Awake()
    {
        // Get the CharacterController component on this GameObject
        characterController = GetComponent<CharacterController>();
    }

    // Public method to move without applying gravity
    public void Move(Vector3 motion)
    {
        // Here we explicitly ignore gravity by not modifying the Y-axis
        motion.y = 0;  // Disable vertical movement caused by gravity

        // Move the character using the CharacterController
        characterController.Move(motion * moveSpeed * Time.deltaTime);
    }
}
