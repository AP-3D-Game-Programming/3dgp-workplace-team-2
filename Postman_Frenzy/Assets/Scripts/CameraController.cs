using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float sensitivity = 2f;
    public float distance = 5f;
    private float rotationX;
    private float rotationY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
        //block player from moving too far
        rotationY = Mathf.Clamp(rotationY, 0, 60f);

        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        //follow after player
        transform.position = player.position + rotation * direction;
        transform.LookAt(player);
    }
}
