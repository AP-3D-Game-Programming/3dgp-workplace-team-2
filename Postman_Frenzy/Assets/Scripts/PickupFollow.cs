using UnityEngine;
using System;

public class PickupFollowFixed : MonoBehaviour
{
    public Transform player;                     // De speler in de scene
    public float pickupRange = 3f;               // Maximale afstand om te kunnen oppakken
    public float followDistance = 2f;            // Afstand waarop het pakket achter de speler zweeft
    public float moveSpeed = 5f;                 // Volgsnelheid
    public KeyCode pickupKey = KeyCode.E;        // Toets om pakket op te pakken / los te laten

    private bool isHeld = false;                 // Of dit pakket momenteel wordt vastgehouden
    private Rigidbody rb;                        // Rigidbody voor fysica
    private PickupPrompt prompt;                 
    private PickupWithSlider timerScript;        
    public DeliveryHouse targetHouse;            
    private MoneyUI moneyUI;                     

    public static event Action<PickupFollowFixed> OnPickup;
    public static event Action<PickupFollowFixed> OnDrop;

    //   Static referentie naar het pakket dat momenteel vastgehouden wordt
    public static PickupFollowFixed currentlyHeldPackage = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        prompt = GetComponent<PickupPrompt>();
        timerScript = GetComponent<PickupWithSlider>();
        moneyUI = UnityEngine.Object.FindFirstObjectByType<MoneyUI>();
    }

    [System.Obsolete]
    void Update()
    {
        //   Als iemand anders al een pakket vasthoudt en dit pakket wordt niet vastgehouden, doe niets
        if (currentlyHeldPackage != null && currentlyHeldPackage != this)
            return;

        if (!isHeld)
        {
            TryPickup(); // Probeer te kijken of speler dit pakket wil oppakken
        }
        else
        {
            FollowPlayer(); // Volg speler als vastgehouden

            if (Input.GetKeyDown(pickupKey))
            {
                DropObject(); // Loslaten
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

        // Alleen oppakken als dicht genoeg én geen ander pakket al vast is
        if (distance <= pickupRange && Input.GetKeyDown(pickupKey) && currentlyHeldPackage == null)
        {
            isHeld = true;
            currentlyHeldPackage = this; //   Registreer dit pakket als actief

            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            GetComponent<Collider>().enabled = false;

            if (prompt != null)
                prompt.isHeld = true;

            if (timerScript != null)
                timerScript.StartTimer();

            if (targetHouse == null)
            {
                SelectRandomHouse();
            }

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

        Debug.Log($"[PickupFollowFixed] Pakket losgelaten, huis blijft actief: {targetHouse?.name}");

        targetHouse.ResetHouse();
        currentlyHeldPackage = null; //   Laat weten dat er weer niets wordt vastgehouden
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
        if (isHeld && targetHouse != null && other.gameObject == targetHouse.gameObject)
        {
            DeliverPackage();
        }
    }

    void DeliverPackage()
    {
        Debug.Log("Pakket bezorgd!");

        if (targetHouse != null)
        {
            targetHouse.isTarget = false;
            targetHouse.ResetHouse();
        }

        isHeld = false;

        if (moneyUI != null)
            moneyUI.AddMoney(50);

        if (prompt != null)
            prompt.HideText();

        HidePrompt();

        currentlyHeldPackage = null; //   Maak weer vrij voor het volgende pakket
        OnDrop?.Invoke(this);

        Destroy(gameObject);
    }

    public void HidePrompt()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        //   Extra zekerheid: als dit pakket verdwijnt, geef de controle vrij
        if (currentlyHeldPackage == this)
            currentlyHeldPackage = null;

        if (targetHouse != null)
        {
            targetHouse.isTarget = false;
            targetHouse.ResetHouse();
        }
    }
}
