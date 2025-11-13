using UnityEngine;
using TMPro;

public class CrateSpawner : MonoBehaviour
{
    [Header("Crate Settings")]
    public GameObject cratePrefab;
    public Transform crateParent;
    public Transform player;
    public TextMeshProUGUI interactText;

    [Header("Spawn Control")]
    public float spawnInterval = 5f;
    public int maxCrates = 5;
    public Vector3 spawnOffset = Vector3.zero;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnCrate();
        }
    }

    void SpawnCrate()
    {
        if (crateParent.childCount >= maxCrates) return;

        Vector3 spawnPos = transform.position + spawnOffset;
        GameObject newCrate = Instantiate(cratePrefab, spawnPos, Quaternion.identity, crateParent);

        CrateHoldScript holdScript = newCrate.GetComponent<CrateHoldScript>();

        if (holdScript != null)
        {
            holdScript.player = player;
            holdScript.interactText = interactText;
        }
        else
        {
            Debug.LogWarning("Crate prefab missing CrateHoldScript!");
        }

        Debug.Log("Spawned crate with references assigned.");
    }
}

