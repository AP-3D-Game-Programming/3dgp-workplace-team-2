using UnityEngine;

public class CompassMarkerAuto : MonoBehaviour
{
    [Header("UI Referenties")]
    public RectTransform compass;
    public RectTransform marker;
    public Transform player;

    private Transform targetHouse;

    void Start()
    {
        if (marker != null)
        {
            marker.gameObject.SetActive(false);
            Debug.Log("[CompassMarkerAuto] Marker hidden at start");
        }
    }

    void OnEnable()
    {
        PickupFollowFixed.OnPickup += HandlePickup;
        PickupFollowFixed.OnDrop += HandleDrop;
    }

    void OnDisable()
    {
        PickupFollowFixed.OnPickup -= HandlePickup;
        PickupFollowFixed.OnDrop -= HandleDrop;
    }

    void HandlePickup(PickupFollowFixed pkg)
    {
        targetHouse = pkg.targetHouse?.transform;
        marker.gameObject.SetActive(targetHouse != null);
        Debug.Log("[CompassMarkerAuto] OnPickup event received! Marker visible = " + (targetHouse != null));
    }

    void HandleDrop(PickupFollowFixed pkg)
    {
        targetHouse = null;
        marker.gameObject.SetActive(false);
        Debug.Log("[CompassMarkerAuto] OnDrop event received! Marker hidden");
    }

    void Update()
    {
        if (player == null || targetHouse == null || compass == null || marker == null)
            return;

        Vector3 dirToTarget = targetHouse.position - player.position;
        dirToTarget.y = 0;

        float angle = Vector3.SignedAngle(player.forward, dirToTarget, Vector3.up);
        float compassWidth = compass.rect.width;
        float markerX = (angle / 90f) * (compassWidth / 2f);
        markerX = Mathf.Clamp(markerX, -compassWidth / 2f, compassWidth / 2f);

        marker.anchoredPosition = new Vector2(markerX, marker.anchoredPosition.y);
    }
}
