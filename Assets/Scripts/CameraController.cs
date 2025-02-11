using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool DeveloperMode = false;
    public Camera mainCamera;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!DeveloperMode)
            mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI");
        else
            mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI");
    }
}
