using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Tutorial : MonoBehaviour
{
[Header("References")]
    public PlayerController player;
    public CarEntry carEntry;
    public TextMeshProUGUI tutorialText;
    public SelectDeliveryHouse deliverySystem; // delivery logic
    public Transform crateParent; // voor alle crates

    [Header("Settings")]
    public float moveThreshold = 0.1f; // minimale afstand voor "bewegen"

    private Vector3 lastPlayerPos;

    private void Start()
    {
        lastPlayerPos = player.transform.position;
        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial()
    {
        // --- Stap 0: Intro ---
        DisableAllControls();
        ShowText("Welkom bij Postman Frenzy! Druk op een toets om te beginnen.");
        yield return WaitForAnyKeyDown();

        // --- Stap 1: Bewegen ---
        ShowText("Stap 1: Gebruik WASD om te bewegen en SPACE om te springen.");
        EnablePlayer();
        yield return new WaitUntil(PlayerHasMoved);
        DisablePlayer();
        ShowText("Goed gedaan!");
        yield return new WaitForSeconds(2);

        // --- Stap 2: Pakket oppakken en neerleggen ---
        ShowText("Stap 2: Loop naar een crate en druk op E om het op te pakken.");
        EnablePlayer();
        yield return new WaitUntil(() =>
        {
            CrateHoldScript[] crates = FindObjectsOfType<CrateHoldScript>();
            foreach (var c in crates)
                if (c.isHeld) return true;
            return false;
        });

        ShowText("Goed! Druk opnieuw op E om het pakket neer te leggen.");
        yield return new WaitUntil(() =>
        {
            CrateHoldScript[] crates = FindObjectsOfType<CrateHoldScript>();
            foreach (var c in crates)
                if (!c.isHeld) return true;
            return false;
        });
        ShowText("Pakket neergelegd!");
        yield return new WaitForSeconds(2);

        // --- Stap 3: Auto in- en uitstappen ---
        ShowText("Stap 3: Loop naar de auto en druk op F om in te stappen. ");
        carEntry.enabled = true;
        yield return new WaitUntil(CarIsEntered);

        ShowText("Je zit in de auto. Movement is hetzelfde als lopen. Druk op F om uit te stappen.");
        yield return new WaitUntil(CarIsExited);
        carEntry.enabled = false;

        // --- Stap 4: Crate inladen ---
        ShowText("Stap 4: Pak een crate en laad deze in de auto door op E te drukken.");
        yield return new WaitUntil(() =>
        {
            CrateHoldScript[] crates = FindObjectsOfType<CrateHoldScript>();
            foreach (var c in crates)
                if (c.isLoadedInVan) return true;
            return false;
        });
        ShowText("Crate is in de auto!");
        yield return new WaitForSeconds(2);

        // --- Stap 5: Crate uitladen ---
        ShowText("Stap 5: Haal de crate eruit door naar de auto te lopen en op E te drukken.");
        yield return new WaitUntil(() =>
        {
            CrateHoldScript[] crates = FindObjectsOfType<CrateHoldScript>();
            foreach (var c in crates)
                if (c.isHeld && !c.isLoadedInVan) return true;
            return false;
        });
        ShowText("Crate uit de auto gehaald!");
        yield return new WaitForSeconds(2);

        // --- Stap 6: Crate afleveren bij het huis ---
        ShowText("Stap 6: Breng het pakket naar het huis en druk op E om het af te leveren.");
        deliverySystem.enabled = true;
        yield return new WaitUntil(() => FindObjectsOfType<CrateHoldScript>().Length == 0);
        ShowText("Geweldig! Het pakket is afgeleverd.");
        yield return new WaitForSeconds(2);

        HideText();
        EnableAllControls();

        SceneManager.LoadScene("MainV2");
    }

    // ----------------------------
    // Helper functies
    // ----------------------------
    private void ShowText(string s)
    {
        tutorialText.text = s;
        tutorialText.gameObject.SetActive(true);
    }

    private void HideText()
    {
        tutorialText.gameObject.SetActive(false);
    }

    private IEnumerator WaitForAnyKeyDown()
    {
        while (!Input.anyKeyDown)
            yield return null;
    }

    private bool PlayerHasMoved()
    {
        float distance = Vector3.Distance(player.transform.position, lastPlayerPos);
        if (distance > moveThreshold)
        {
            lastPlayerPos = player.transform.position;
            return true;
        }
        return false;
    }

    private bool CarIsEntered()
    {
        var type = carEntry.GetType();
        var field = type.GetField("isInVehicle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            return (bool)field.GetValue(carEntry);
        return false;
    }

    private bool CarIsExited()
    {
        return !CarIsEntered();
    }

    private void DisableAllControls()
    {
        player.enabled = false;
        carEntry.enabled = false;
        deliverySystem.enabled = false;
    }

    private void EnableAllControls()
    {
        player.enabled = true;
        carEntry.enabled = true;
        deliverySystem.enabled = true;
    }

    private void EnablePlayer()
    {
        player.enabled = true;
    }

    private void DisablePlayer()
    {
        player.enabled = false;
    }
}
