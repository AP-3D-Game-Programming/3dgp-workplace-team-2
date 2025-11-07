using UnityEngine;

public class PickupFollowFixed : MonoBehaviour
{
    public Transform player;
    public float pickupRange = 3f;
    public float followDistance = 2f;
    public float moveSpeed = 5f;
    public KeyCode pickupKey = KeyCode.E;

    private bool isHeld = false;
    private Rigidbody rb;
    private PickupPrompt prompt;
    private PickupWithSlider timerScript;

    public DeliveryHouse targetHouse; // Made public so other scripts can access it
    private MoneyUI moneyUI;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        prompt = GetComponent<PickupPrompt>();
        timerScript = GetComponent<PickupWithSlider>();
        moneyUI = Object.FindFirstObjectByType<MoneyUI>(); // <— zoek UI-script
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
        targetPosition.y = transform.position.y;

        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
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
                prompt.isHeld = true;

            if (timerScript != null)
                timerScript.StartTimer();

            SelectRandomHouse();
        }
    }

    void DropObject()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = true;

        if (prompt != null)
            prompt.isHeld = false;

        if (targetHouse != null)
            targetHouse.isTarget = false; // reset highlight
    }

    void SelectRandomHouse()
    {
        DeliveryHouse[] houses = Object.FindObjectsByType<DeliveryHouse>(FindObjectsSortMode.None);
        if (houses.Length == 0) return;

        targetHouse = houses[Random.Range(0, houses.Length)];
        targetHouse.isTarget = true;
        Debug.Log($"Nieuw doelhuis: {targetHouse.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (isHeld && targetHouse != null && other.gameObject == targetHouse.gameObject)
        {
            DeliverPackage();
        }
    }

    void DeliverPackage()
    {
        Debug.Log("Pakket bezorgd!");
        targetHouse.isTarget = false;
        isHeld = false;

        if (moneyUI != null)
            moneyUI.AddMoney(50); // bijvoorbeeld €50

        if (prompt != null)
            prompt.HideText(); // <— verberg de UI tekst meteen

        Destroy(gameObject); // verwijder het pakket
    }

    public void HidePrompt()
    {
        gameObject.SetActive(false);
    }
}
