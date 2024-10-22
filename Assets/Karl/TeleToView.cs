using UnityEngine;
using UnityEngine.InputSystem; // New Input System namespace for input actions

public class TeleToView : MonoBehaviour
{
    // Reference to the XR Camera (main camera or player's head in VR)
    public Transform xrCamera;

    // Input Action for toggling (space bar and/or VR controller button)
    public InputAction toggleAction;

    // Public distance for how far in front of the camera the object should teleport
    public float planeDistance = 5.0f;

    // Reference to the AlignToPlayerView script
    private AlignToPlayerView alignToPlayerView;

    // Boolean to track whether this object is parented to the XR Camera
    private bool isParented = false;

    private void Awake()
    {
        // Get the AlignToPlayerView component on the same GameObject
        alignToPlayerView = GetComponent<AlignToPlayerView>();
    }

    private void OnEnable()
    {
        // Enable the input action and register the callback
        toggleAction.Enable();
        toggleAction.performed += OnToggleAction;
    }

    private void OnDisable()
    {
        // Unregister the callback and disable the input action
        toggleAction.performed -= OnToggleAction;
        toggleAction.Disable();
    }

    // Called when the toggle action is performed (Spacebar or VR button press)
    private void OnToggleAction(InputAction.CallbackContext context)
    {
        // Toggle the alignToView boolean in the AlignToPlayerView script
        if (alignToPlayerView != null)
        {
            alignToPlayerView.alignToView = !alignToPlayerView.alignToView;
            Debug.Log($"AlignToView toggled. Now: {alignToPlayerView.alignToView}");
        }

        // Toggle the parenting of this GameObject to or from the XR Camera
        ToggleParenting();
    }

    // Toggle whether this GameObject is parented to the XR Camera
    private void ToggleParenting()
    {
        if (xrCamera == null)
        {
            Debug.LogError("XR Camera reference is missing!");
            return;
        }

        if (isParented)
        {
            // Unparent the GameObject from the XR Camera
            transform.SetParent(null);
            Debug.Log("GameObject unparented from XR Camera.");
        }
        else
        {
            // Teleport the object to planeDistance units in front of the camera
            TeleportInFrontOfCamera();

            // Parent the GameObject to the XR Camera
            transform.SetParent(xrCamera);
            Debug.Log("GameObject parented to XR Camera.");
        }

        // Toggle the parenting state
        isParented = !isParented;
    }

    // Teleports the GameObject to be a certain distance in front of the XR Camera
    private void TeleportInFrontOfCamera()
    {
        // Calculate the new position based on the camera's forward direction and planeDistance
        Vector3 cameraForward = xrCamera.forward;
        Vector3 newPosition = xrCamera.position + cameraForward * planeDistance;

        // Move the GameObject to the calculated position
        transform.position = newPosition;

        Debug.Log($"GameObject teleported {planeDistance} units in front of XR Camera.");
    }
}
