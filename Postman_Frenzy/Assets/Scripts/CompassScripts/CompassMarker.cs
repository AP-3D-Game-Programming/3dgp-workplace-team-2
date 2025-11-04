using UnityEngine;
using UnityEngine.UI;

public class CompassMarker : MonoBehaviour
{
    [Header("Referenties")]
    public Transform player;
    public Transform target; // het doelhuis
    public RectTransform compass; // UI-kompas (de RawImage zelf)
    public RectTransform marker;  // het bolletje dat de richting toont

    void Update()
    {
        if (player == null || target == null || compass == null || marker == null)
            return;

        // Bereken richting van speler naar target in wereld
        Vector3 dirToTarget = target.position - player.position;
        dirToTarget.y = 0; // negeer hoogte

        // Bepaal hoek tussen speler en doelhuis
        float angle = Vector3.SignedAngle(player.forward, dirToTarget, Vector3.up);

        // Vertaal hoek naar positie op kompas (breedte)
        float compassWidth = compass.rect.width;
        float markerX = (angle / 90f) * (compassWidth / 2f); // 90° = halve breedte

        // Clampen zodat het niet te ver uit beeld gaat
        markerX = Mathf.Clamp(markerX, -compassWidth / 2f, compassWidth / 2f);

        // Update markerpositie
        marker.anchoredPosition = new Vector2(markerX, marker.anchoredPosition.y);
    }
}
