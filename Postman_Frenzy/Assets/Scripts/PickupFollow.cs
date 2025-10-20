using UnityEngine;

public class PickupFollowFixed : MonoBehaviour
{
    public Transform player;          // speler waar het object achteraan volgt
    public float pickupRange = 3f;    // afstand om op te pakken
    public float followDistance = 2f; // hoe ver achter de speler het object volgt
    public float moveSpeed = 5f;      // snelheid van volgen
    public KeyCode pickupKey = KeyCode.E;

    private bool isHeld = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // kantelen voorkomen
    }

    [System.Obsolete]
    void Update()
    {
        if (!isHeld)
        {
            TryPickup();
        }
        else
        {
            FollowPlayer();

            if (Input.GetKeyDown(pickupKey))
            {
                DropObject();
            }
        }
    }

    [System.Obsolete]
    void TryPickup()
    {
        // Alleen oppakken als dichtbij speler
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= pickupRange && Input.GetKeyDown(pickupKey))
        {
            isHeld = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }

    void FollowPlayer()
    {
        // Bepaal target positie achter de speler
        Vector3 targetPosition = player.position - player.forward * followDistance;
        targetPosition.y = transform.position.y; // hoogte behouden

        // Smooth movement met Lerp i.p.v. velocity
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition); // physics-safe verplaatsing
    }

    [System.Obsolete]
    void DropObject()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
    }
}
