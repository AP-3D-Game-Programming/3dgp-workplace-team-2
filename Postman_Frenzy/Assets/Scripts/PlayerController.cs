using NUnit.Framework;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    public Vector3 jump;
    public float jumpForce = 2.0f;
    public bool isGround;
    Rigidbody rb;

    public float speed = 5f;
    public Transform cameraPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //player movement
        //player jump
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraPos.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = rotation;

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            transform.position += moveDir * speed * Time.deltaTime;
        }
    }
    void OnCollisionStay()
    {
        isGround = true;
    }
    void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }
}
