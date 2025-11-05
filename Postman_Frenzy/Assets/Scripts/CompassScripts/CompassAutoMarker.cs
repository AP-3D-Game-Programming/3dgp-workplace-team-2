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
    // Kijk of de speler een actief pakket vasthoudt
    var heldPackage = FindAnyObjectByType<PickupFollowFixed>();

    if (heldPackage != null && heldPackage.targetHouse != null)
    {
        targetHouse = heldPackage.targetHouse.transform;
    }
    else
    {
        targetHouse = null; // Geen actief pakket → marker verbergen
    }

    // Marker verbergen als er geen doelhuis is
    if (marker != null)
        marker.gameObject.SetActive(targetHouse != null);

    if (player == null || targetHouse == null || compass == null || marker == null)
        return;

    // Bereken richting van speler naar doelhuis
    Vector3 dirToTarget = targetHouse.position - player.position;
    dirToTarget.y = 0;

    float angle = Vector3.SignedAngle(player.forward, dirToTarget, Vector3.up);

    float compassWidth = compass.rect.width;
    float markerX = (angle / 90f) * (compassWidth / 2f);
    markerX = Mathf.Clamp(markerX, -compassWidth / 2f, compassWidth / 2f);

    marker.anchoredPosition = new Vector2(markerX, marker.anchoredPosition.y);
}
}
