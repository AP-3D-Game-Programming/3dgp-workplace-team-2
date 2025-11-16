using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    public Camera playerCamera;   // your main camera
    public Camera topCamera;      // second camera above the sphere

    private bool usingTopCamera = false;

    void Start()
    {
        topCamera.enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            usingTopCamera = !usingTopCamera;

            playerCamera.enabled = !usingTopCamera;
            topCamera.enabled = usingTopCamera;
        }
    }
}