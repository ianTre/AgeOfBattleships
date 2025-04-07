using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float rotateSpeed = 5.0f; // Speed of rotation
    public bool isRotating = false; // Flag to control rotation
    public static CameraRotator instance; // Singleton instance
    [SerializeField]
    Camera cameraToRotate; // Reference to the camera to rotate
    private Camera previousCamera; // Reference to the previous camera
    /// <summary>
    /// Singleton pattern to ensure only one instance of CameraRotator exists.
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this; // Initialize the singleton instance
    }

    void Update()
    {
        if(isRotating)
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0); // Rotate around the Y-axis
    }

    public void StartRotation(Vector3 position)
    {
        if (cameraToRotate != null)
        {
            previousCamera = Camera.main; // Store the previous camera
            previousCamera.gameObject.SetActive(false); // Deactivate the previous camera
            cameraToRotate.gameObject.SetActive(true); // Deactivate the previous camera
        }
        transform.position = position; // Set the camera position
        transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation to default
        isRotating = true; // Start rotating
    }

    public void StopRotation()
    {
        isRotating = false; // Stop rotating
        if (previousCamera != null)
        {
            cameraToRotate.gameObject.SetActive(false); // Deactivate the camera to rotate
            previousCamera.gameObject.SetActive(true); // Reactivate the previous camera
        }
        previousCamera = null; // Clear the reference to the previous camera
    }
}
