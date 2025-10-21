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
    private PickupPrompt prompt;      // verwijzing naar het prompt-script

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Zoek het PickupPrompt-script op hetzelfde object
        prompt = GetComponent<PickupPrompt>();
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

    void FollowPlayer()
    {
        Vector3 targetPosition = player.position - player.forward * followDistance;
        targetPosition.y = transform.position.y; // hoogte behouden

        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition); // physics-safe verplaatsing
    }

    void TryPickup()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= pickupRange && Input.GetKeyDown(pickupKey))
        {
            isHeld = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            GetComponent<Collider>().enabled = false;

            if (prompt != null)
                prompt.isHeld = true; // update de prompt
        }
    }

    void DropObject()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = true;

        if (prompt != null)
            prompt.isHeld = false; // update de prompt
    }
}
