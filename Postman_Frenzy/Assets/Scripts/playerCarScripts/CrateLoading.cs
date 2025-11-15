using UnityEngine;
using TMPro;
public class CrateLoading : MonoBehaviour
{
    private bool playerInside = false;
    public TextMeshProUGUI loadCrateTxt;
    void Start()
    {
        loadCrateTxt.gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            loadCrateTxt.gameObject.SetActive(true);
            Debug.Log("Player entered van loading zone");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            loadCrateTxt.gameObject.SetActive(false);
            Debug.Log("Player left van loading zone");
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            CrateHoldScript heldCrate = FindHeldCrate();
            CrateHoldScript loadedCrate = FindLoadedCrate();

            if (heldCrate != null)
            {
                // Load crate into van
                heldCrate.TryLoadOrUnload();
            }
            else if (loadedCrate != null)
            {
                // Unload crate from van
                loadedCrate.TryLoadOrUnload();
            }
        }
    }

    CrateHoldScript FindHeldCrate()
    {
        CrateHoldScript[] allCrates = FindObjectsByType<CrateHoldScript>(FindObjectsSortMode.None);
        foreach (var crate in allCrates)
        {
            if (crate.isHeld)
                return crate;
        }
        return null;
    }

    CrateHoldScript FindLoadedCrate()
    {
        CrateHoldScript[] allCrates = FindObjectsByType<CrateHoldScript>(FindObjectsSortMode.None);
        foreach (var crate in allCrates)
        {
            if (crate.isLoadedInVan)
                return crate;
        }
        return null;
    }
}
