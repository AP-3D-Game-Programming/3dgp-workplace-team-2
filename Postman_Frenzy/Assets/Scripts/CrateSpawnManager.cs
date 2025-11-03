using UnityEngine;

public class CrateSpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject originalCrate;  // Reference to the original crate in the scene

    [SerializeField]
    private Vector3 spawnPosition;  // Position where crates will spawn

    private GameObject currentCrate;  // Reference to the current crate in the scene
    private GameObject crateTemplate; // Stored template of the original crate

    private void Start()
    {
        if (originalCrate == null)
        {
            Debug.LogError("Original crate reference is missing! Make sure to assign a crate in the inspector.");
            return;
        }

        // Store the spawn position from the original crate if not set
        if (spawnPosition == Vector3.zero)
        {
            spawnPosition = originalCrate.transform.position;
        }

        // Create a template copy and hide it
        crateTemplate = Instantiate(originalCrate);
        crateTemplate.SetActive(false);
        DontDestroyOnLoad(crateTemplate); // Keep the template across scene loads

        // The original crate becomes our first current crate
        currentCrate = originalCrate;
    }

    private void Update()
    {
        // If there is no current crate (it was destroyed), spawn a new one
        if (currentCrate == null)
        {
            SpawnCrate();
        }
    }

    private void SpawnCrate()
    {
        if (crateTemplate != null)
        {
            // Create a copy of the template at the spawn position
            GameObject newCrate = Instantiate(crateTemplate, spawnPosition, crateTemplate.transform.rotation);
            newCrate.SetActive(true);
            currentCrate = newCrate;
        }
        else
        {
            Debug.LogError("Crate template is missing! This shouldn't happen - please check the CrateSpawnManager setup.");
        }
    }

    private void OnDestroy()
    {
        // Clean up the template when the manager is destroyed
        if (crateTemplate != null)
        {
            Destroy(crateTemplate);
        }
    }
}
