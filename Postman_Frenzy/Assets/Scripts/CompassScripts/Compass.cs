using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage compassImage;
    public Transform player;

    void Update()
    {
        if (player == null || compassImage == null) return;

        float playerRotation = player.eulerAngles.y;

        // Offset berekenen en toepassen op UV rect
        compassImage.uvRect = new Rect(playerRotation / 360f, 0f, 1f, 1f);
    }
}
