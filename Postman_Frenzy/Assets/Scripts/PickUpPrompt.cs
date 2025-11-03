using UnityEngine;
using UnityEngine.UI;

public class PickupPrompt : MonoBehaviour
{
    [Header("UI Elements")]
    public Text promptText; // Tekst die op het scherm wordt getoond

    [Header("Settings")]
    public string playerTag = "Player"; // Tag van de speler
    public string pickupMessage = "Press 'E' to pick up";
    public string dropMessage = "Press 'E' to drop";
    public string deliverMessage = "Press 'E' to deliver package";
    public float showDistance = 3f;

    [HideInInspector]
    public bool isHeld = false;

    private Transform player;
    private PickupFollowFixed pickupFollow;
    private PlayerDeliveryTrigger deliveryTrigger; // <— referentie naar speler-trigger

    void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);

        // Zoek speler en scripts
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
            deliveryTrigger = playerObj.GetComponent<PlayerDeliveryTrigger>(); // haal script op
        }

        pickupFollow = GetComponent<PickupFollowFixed>();
    }

    void Update()
    {
        if (player == null || promptText == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= showDistance || isHeld)
        {
            string textToShow = pickupMessage;

            if (isHeld)
            {
                // Controleer of speler bij doelhuis staat
                bool nearTarget = false;

                if (deliveryTrigger != null && pickupFollow != null)
                {
                    // Check of speler momenteel bij het juiste huis is
                    var field = deliveryTrigger.GetType().GetField("currentHouse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    DeliveryHouse currentHouse = field?.GetValue(deliveryTrigger) as DeliveryHouse;

                    if (currentHouse != null && currentHouse == pickupFollow.targetHouse)
                        nearTarget = true;
                }

                // Toon juiste tekst
                textToShow = nearTarget ? deliverMessage : dropMessage;
            }

            promptText.text = textToShow;
            promptText.gameObject.SetActive(true);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
        if (player == null || promptText == null) return;

        Debug.Log($"Distance to player: {distance}");
    }

    public void HideText()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
}
