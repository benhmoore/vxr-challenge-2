using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 5.0f;               // Movement speed
    private Vector2 playerMovementInput;     // Input values for movement

    void Update()
    {
        // Call movement method every frame
        MovePlayer();
    }

    void MovePlayer()
    {
        // Convert the 2D input into a 3D movement vector (no Y-axis movement)
        Vector3 movement = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.y);

        // Move the player using the transform (no need for a CharacterController)
        // Multiply by speed and Time.deltaTime for consistent movement
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // This method gets called when movement input is detected (via the Input System)
    void OnMove(InputValue iv)
    {
        // Update player movement input vector
        playerMovementInput = iv.Get<Vector2>();
    }
}
