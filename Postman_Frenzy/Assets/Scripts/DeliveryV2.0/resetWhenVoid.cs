using UnityEngine;

public class CrateReset : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    public Transform water;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        if (water == null)
            water = GameObject.FindWithTag("Water").transform;
    }

    void Update()
    {
        if (water != null && transform.position.y < water.position.y - 2)
        {
            ResetCrate();
        }
    }

    void ResetCrate()
    {
        transform.position = startPos;
        transform.rotation = startRot;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
