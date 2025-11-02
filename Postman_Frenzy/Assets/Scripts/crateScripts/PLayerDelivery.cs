using UnityEngine;

public class PlayerDeliveryTrigger : MonoBehaviour
{
    private PickupFollowFixed heldPackage;

    void Update()
    {
        // Kijk of speler een pakket vasthoudt
        if (heldPackage == null)
            heldPackage = FindAnyObjectByType<PickupFollowFixed>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (heldPackage != null && other.TryGetComponent(out DeliveryHouse house))
        {
            if (house.isTarget)
            {
                Debug.Log("Speler heeft doelhuis bereikt!");
                heldPackage.SendMessage("DeliverPackage"); // roept de functie in de crate aan
            }
        }
    }
}
