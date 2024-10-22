using UnityEngine;

public class AlignToPlayerView : MonoBehaviour
{
    // Public boolean to toggle whether to align to the main camera's orientation
    public bool alignToView = true;

    // Reference to the main camera's transform
    private Transform mainCameraTransform;

    void Start()
    {
        // Find the main camera at the start
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Check if alignToView is enabled
        if (alignToView)
        {
            // Continuously align this object's rotation to the main camera's rotation
            AlignToCamera();
        }
        // When alignToView is false, do nothing and keep the current rotation
    }

    // Aligns the object's rotation to the main camera's rotation
    private void AlignToCamera()
    {
        if (mainCameraTransform != null)
        {
            // Copy the main camera's orientation (rotation)
            transform.rotation = mainCameraTransform.rotation;
        }
    }
}
