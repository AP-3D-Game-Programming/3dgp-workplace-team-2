using UnityEngine;

public class PlayerDeliveryTrigger : MonoBehaviour
{
    private PickupFollowFixed heldPackage; // Referentie naar het pakket dat speler vasthoudt
    private DeliveryHouse currentHouse;    // Houdt bij bij welk huis we staan

    void Update()
    {
        // Zoek actief of speler een pakket vasthoudt
        if (heldPackage == null)
            heldPackage = FindAnyObjectByType<PickupFollowFixed>();

        // Alleen doorgaan als speler iets vasthoudt
        if (heldPackage != null && heldPackage.isActiveAndEnabled)
        {
            // ✅ Controleer of speler bij een huis is en op E drukt
            if (currentHouse != null && currentHouse == heldPackage.targetHouse)
            {
                // Toets controleren
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Speler heeft E gedrukt om te leveren!");
                    heldPackage.SendMessage("DeliverPackage"); // Lever pakket via functie
                    currentHouse = null; // Reset huis
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Controleer of we een huis betreden
        if (other.TryGetComponent(out DeliveryHouse house))
        {
            // Enkel als dit het doelhuis is
            if (heldPackage != null && house == heldPackage.targetHouse)
            {
                Debug.Log("Speler is in de buurt van doelhuis!");
                currentHouse = house; // Onthoud dat speler nu bij het doelhuis is
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Als speler het huis verlaat, vergeet het
        if (other.TryGetComponent(out DeliveryHouse house))
        {
            if (house == currentHouse)
            {
                Debug.Log("Speler heeft het huis verlaten");
                currentHouse = null;
            }
        }
    }
}
