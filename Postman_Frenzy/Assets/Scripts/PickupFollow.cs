using UnityEngine;
using System;

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
    public DeliveryHouse targetHouse;            
    private MoneyUI moneyUI;                     

    public static event Action<PickupFollowFixed> OnPickup;
    public static event Action<PickupFollowFixed> OnDrop;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        prompt = GetComponent<PickupPrompt>();
        timerScript = GetComponent<PickupWithSlider>();
        moneyUI = UnityEngine.Object.FindFirstObjectByType<MoneyUI>();
        // ❌ Geen huis selecteren bij Start(), dat gebeurt pas bij pickup.
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

            // ✅ Kies huis alleen bij de eerste keer oppakken
            if (targetHouse == null)
            {
                SelectRandomHouse();
            }

            // ✅ Zet dat huis groen
            if (targetHouse != null)
                targetHouse.isTarget = true;

            Debug.Log($"[PickupFollowFixed] Huis geselecteerd en groen gemaakt: {targetHouse.name}");

            OnPickup?.Invoke(this);
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

        // ❌ NIET resetten bij droppen, het huis blijft actief
        Debug.Log($"[PickupFollowFixed] Pakket losgelaten, huis blijft actief: {targetHouse?.name}");
        targetHouse.ResetHouse();
        OnDrop?.Invoke(this);
    }

    void SelectRandomHouse()
    {
        DeliveryHouse[] houses = UnityEngine.Object.FindObjectsByType<DeliveryHouse>(FindObjectsSortMode.None);
        if (houses.Length == 0) return;

        targetHouse = houses[UnityEngine.Random.Range(0, houses.Length)];
        Debug.Log($"[PickupFollowFixed] Nieuw doelhuis ingesteld bij eerste oppak: {targetHouse.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        // ✅ Alleen leveren als speler het juiste huis bereikt met het pakket in handen
        if (isHeld && targetHouse != null && other.gameObject == targetHouse.gameObject)
        {
            DeliverPackage();
        }
    }

    void DeliverPackage()
    {
        Debug.Log("Pakket bezorgd!");

        // ✅ Zet het huis terug naar standaardkleur
        if (targetHouse != null)
        {
            targetHouse.isTarget = false;
            targetHouse.ResetHouse(); // roept veilig kleurreset aan
        }

        isHeld = false;

        if (moneyUI != null)
            moneyUI.AddMoney(50);

        if (prompt != null)
            prompt.HideText();

        HidePrompt();

        OnDrop?.Invoke(this);

        // ✅ Verwijder het pakket na aflevering
        Destroy(gameObject);
    }

    public void HidePrompt()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // ✅ Extra failsafe: reset huiskleur bij vernietiging
        if (targetHouse != null)
        {
            targetHouse.isTarget = false;
            targetHouse.ResetHouse();
        }
    }
}
