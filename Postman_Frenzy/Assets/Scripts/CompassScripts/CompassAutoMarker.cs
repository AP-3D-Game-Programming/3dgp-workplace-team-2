using UnityEngine;

public class CompassMarkerAuto : MonoBehaviour
{
    [Header("UI Referenties")]
    public RectTransform compass;   // je kompasstrip RawImage
    public RectTransform marker;    // bolletje prefab / Image
    public Transform player;        // speler of camera

    private Transform targetHouse;  // automatisch doelhuis

   void Update()
    {
        // Probeer het doelhuis automatisch te vinden
        if (targetHouse == null)
        {
            // Kijk of de speler een pakket vasthoudt
            var heldPackage = FindAnyObjectByType<PickupFollowFixed>();
            if (heldPackage != null && heldPackage.targetHouse != null)
            {
                targetHouse = heldPackage.targetHouse.transform; // <- hier de fix
            }
        }

        if (player == null || targetHouse == null || compass == null || marker == null)
            return;

        // Bereken richting van speler naar doelhuis
        Vector3 dirToTarget = targetHouse.position - player.position;
        dirToTarget.y = 0;

        // Hoek tussen speler en huis
        float angle = Vector3.SignedAngle(player.forward, dirToTarget, Vector3.up);

        // Vertaal hoek naar positie op kompas
        float compassWidth = compass.rect.width;
        float markerX = (angle / 90f) * (compassWidth / 2f);

        // Clamp zodat marker niet buiten beeld gaat
        markerX = Mathf.Clamp(markerX, -compassWidth / 2f, compassWidth / 2f);

        // Update markerpositie
        marker.anchoredPosition = new Vector2(markerX, marker.anchoredPosition.y);
    }
}
