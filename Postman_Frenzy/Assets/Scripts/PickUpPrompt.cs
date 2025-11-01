using UnityEngine;
using UnityEngine.UI;

public class PickupPrompt : MonoBehaviour
{
    [Header("UI Elements")]
    public Text promptText;              // Tekst die we tonen op het scherm

    [Header("Settings")]
    public string playerTag = "Player";  // Tag van de speler
    public string pickupMessage = "Press 'E' to pick up"; // Tekst als object ligt
    public string dropMessage = "Press 'E' to drop";      // Tekst als object wordt vastgehouden
    public float showDistance = 3f;      // Afstand waarop de tekst zichtbaar wordt

    [HideInInspector] 
    public bool isHeld = false;          // Wordt aangepast door PickupFollowFixed

    private Transform player;

    void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false); // tekst eerst verbergen

        // Zoek automatisch de speler
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
        if (distance <= showDistance || isHeld) // ook tonen als object wordt vastgehouden
        {
            // Kies de juiste boodschap
            promptText.text = isHeld ? dropMessage : pickupMessage;
            promptText.gameObject.SetActive(true);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }

    public void HideText()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
}
