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
        //horizontal axis
        float XAis = Input.GetAxis("Horizontal");
        //vertical axis
        float ZAxis = Input.GetAxis("Vertical");
        //movement vector
        Vector3 movement = new Vector3(XAis, 0f, ZAxis);
        //movement
        transform.Translate(movement * speed * Time.deltaTime);
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
