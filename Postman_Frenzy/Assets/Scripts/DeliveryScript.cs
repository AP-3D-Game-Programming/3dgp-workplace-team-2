using UnityEngine;
using UnityEngine.UI;

public class DeliverySystem : MonoBehaviour
{
    [Header("Huis instellingen")]
    public Transform pickupHouse;       // Huis waar je het pakketje oppakt
    public Transform deliveryHouse;     // Huis waar je het pakketje aflevert
    public float interactDistance = 3f; // Afstand waarop je kunt interacteren

    [Header("UI")]
    public Text promptText;             // Tekst op het scherm
    public GameObject packagePrefab;    // Het pakketje dat je oppakt
    private GameObject heldPackage;     // Huidige pakketje

    private bool hasPackage = false;

    void Update()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);

        // Check afstand tot pickup house
        float distanceToPickup = Vector3.Distance(transform.position, pickupHouse.position);
        if (!hasPackage && distanceToPickup <= interactDistance)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = "Press 'E' to pick up package";

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpPackage();
            }
            return;
        }

        // Check afstand tot delivery house
        float distanceToDelivery = Vector3.Distance(transform.position, deliveryHouse.position);
        if (hasPackage && distanceToDelivery <= interactDistance)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = "Press 'E' to deliver package";

            if (Input.GetKeyDown(KeyCode.E))
            {
                DeliverPackage();
            }
        }
    }

    void PickUpPackage()
    {
        hasPackage = true;
        // Maak pakketje aan en laat het bijvoorbeeld boven de speler zweven
        heldPackage = Instantiate(packagePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        heldPackage.transform.SetParent(transform); // laat pakketje mee bewegen met speler
    }

    void DeliverPackage()
    {
        hasPackage = false;
        if (heldPackage != null)
        {
            Destroy(heldPackage);
            heldPackage = null;
        }
        Destroy(heldPackage);
        heldPackage = null;

        // Hier kun je eventueel score toevoegen of iets triggeren
        Debug.Log("Package delivered!");
    }
}
