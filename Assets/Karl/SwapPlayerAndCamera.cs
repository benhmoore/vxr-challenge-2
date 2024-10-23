using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System namespace

public class SwapPlayerAndCamera : MonoBehaviour
{
    // Public reference to the VirtualCamera
    public GameObject VirtualCamera, xrPlayer, xrCameraOffset, xrCamera;
    public AudioSource headsetSource;
    public AudioClip teleportSound;

    // Input Action for swapping via key press
    public InputAction swapAction;

    private void OnEnable()
    {
        // Enable the input action when the script is enabled
        swapAction.Enable();

        // Register the event for when the action is triggered
        swapAction.performed += OnSwapActionTriggered;
    }

    private void OnDisable()
    {
        // Unregister and disable the input action when the script is disabled
        swapAction.performed -= OnSwapActionTriggered;
        swapAction.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has a specific component (class)
        if (other.gameObject.tag == "XRPlayer" || other.gameObject.tag == "MainCamera" || other.gameObject == xrCamera)
        {
            // Swap positions between the XR Player and the VirtualCamera
            Debug.Log($"Collision detected: " + other.gameObject.name);
            Debug.Log($"Collision detected: " + other.gameObject.GetType());
            SwapPositions(xrPlayer, VirtualCamera);
        }
    }


    // This method will be called when the key press (swapAction) is triggered
    private void OnSwapActionTriggered(InputAction.CallbackContext context)
    {
        if (xrPlayer != null && VirtualCamera != null)
        {
            // Fire the swap and print to console
            //Debug.Log($"Swap action triggered. Swapping positions of XR Player and Virtual Camera.");
            SwapPositions(xrPlayer, VirtualCamera);
        }
        else
        {
            Debug.LogError("XR Player or Virtual Camera is not assigned or found!");
        }
    }

    private void SwapPositions(GameObject xrPlayer, GameObject virtualCamera)
    {
        // Disable the CharacterController if it exists on the camera
        CharacterController cameraController = virtualCamera.GetComponent<CharacterController>();
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }

        // Store the positions of both the XR Player and VirtualCamera
        Vector3 xrPlayerPosition = xrPlayer.transform.position;
        Vector3 virtualCameraPosition = virtualCamera.transform.position;

        // Swap the positions
        xrPlayer.transform.position = virtualCameraPosition;
        virtualCamera.transform.position = xrPlayerPosition;

        // Adjust for the camera offset
        xrPlayer.transform.position = new Vector3(xrPlayer.transform.position.x, xrPlayer.transform.position.y + (xrPlayer.transform.position.y - xrCameraOffset.transform.position.y), xrPlayer.transform.position.z);

        VirtualCamera.transform.position = new Vector3(VirtualCamera.transform.position.x, VirtualCamera.transform.position.y - (xrPlayer.transform.position.y - xrCameraOffset.transform.position.y), VirtualCamera.transform.position.z);

        // Re-enable the CharacterController after moving
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        headsetSource.PlayOneShot(teleportSound);
    }

}
