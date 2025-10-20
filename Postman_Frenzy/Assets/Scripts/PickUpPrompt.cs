using UnityEngine;
using UnityEngine.UI;

public class PickupPrompt : MonoBehaviour
{
    [Header("UI Elements")]
    public Text promptText;              // Tekst die we tonen op het scherm

    [Header("Settings")]
    public string playerTag = "Player";  // Tag van de speler
    public string message = "Press 'E' to pick up"; // Bericht dat wordt getoond
    public float showDistance = 3f;      // Afstand waarop de tekst zichtbaar wordt

    private Transform player;

    void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);

        // Zoek de speler automatisch (optioneel)
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null || promptText == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Toon bericht alleen als speler dichtbij genoeg is
        if (distance <= showDistance)
        {
            promptText.text = message;
            promptText.gameObject.SetActive(true);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
