using UnityEngine;
using UnityEngine.InputSystem; // New Input System namespace

public class SwapPlayerAndCamera : MonoBehaviour
{
    // Public reference to the VirtualCamera
    public GameObject VirtualCamera, xrPlayer, xrCameraOffset;

    // Tag for the XR Player. You can customize this or set it in the Unity Editor.
    public string xrPlayerTag = "XRPlayer";

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
        // Check if the object entering the trigger has the XR Player tag
        if (other.CompareTag(xrPlayerTag))
        {
            // Get the XR Player GameObject
            xrPlayer = other.gameObject;

            // Swap positions between the XR Player and the VirtualCamera
            SwapPositions(xrPlayer, VirtualCamera);
        }
    }

    // This method will be called when the key press (swapAction) is triggered
    private void OnSwapActionTriggered(InputAction.CallbackContext context)
    {
        // Try to find the XR player using its tag if it's not already set
        if (xrPlayer == null)
        {
            xrPlayer = GameObject.FindGameObjectWithTag(xrPlayerTag);
        }

        if (xrPlayer != null && VirtualCamera != null)
        {
            // Fire the swap and print to console
            Debug.Log($"Swap action triggered. Swapping positions of XR Player and Virtual Camera.");
            SwapPositions(xrPlayer, VirtualCamera);
        }
        else
        {
            Debug.LogError("XR Player or Virtual Camera is not assigned or found!");
        }
    }

    private void SwapPositions(GameObject xrPlayer, GameObject virtualCamera)
    {
        if (xrPlayer == null || virtualCamera == null)
        {
            Debug.LogError("XR Player or Virtual Camera is not assigned!");
            return;
        }

        // Store the positions of both the XR Player and VirtualCamera
        Vector3 xrPlayerPosition = xrPlayer.transform.position;
        Vector3 virtualCameraPosition = virtualCamera.transform.position;

        // Swap the positions
        xrPlayer.transform.position = virtualCameraPosition;
        virtualCamera.transform.position = xrPlayerPosition;

        //push the xrOrigin down to account for the offset
        xrPlayer.transform.position = new Vector3(xrPlayer.transform.position.x, xrPlayer.transform.position.y + (xrPlayer.transform.position.y - xrCameraOffset.transform.position.y), xrPlayer.transform.position.z); ;

        //push the virtualCamera up to account for the offset
        VirtualCamera.transform.position = new Vector3(VirtualCamera.transform.position.x, VirtualCamera.transform.position.y - (xrPlayer.transform.position.y - xrCameraOffset.transform.position.y), VirtualCamera.transform.position.z);

        //Debug.Log("Swapped positions of XR Player and Virtual Camera.");
    }
}
