using UnityEngine;
using TMPro;
using System;

public class CrateSpawner : MonoBehaviour
{
    [Header("Crate Settings")]
    public GameObject cratePrefab;
    public Transform crateParent;
    public Transform player;
    public TextMeshProUGUI interactText;
    public Transform congratulations;
    public int crateAmound;

    [Header("Spawn Control")]
    public float spawnInterval = 5f;
    public int maxCrates = 5;
    public Vector3 spawnOffset = Vector3.zero;

    private Boolean firstTime = true;
    private float timer;
    void Start()
    {
        congratulations.gameObject.SetActive(false);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnCrate();
        }
        if (firstTime && crateAmound == 6)
        {
            congratulations.gameObject.SetActive(true);
            firstTime = false; // only show once
        }

        // Check for hiding
        if (congratulations && Input.GetKeyDown(KeyCode.E))
        {
            congratulations.gameObject.SetActive(false);
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
        crateAmound += 1;

        if (firstTime == true && crateAmound == 1)
        {
            congratulations.gameObject.SetActive(true);
            firstTime = false;
            if (Input.GetKeyDown(KeyCode.E))
            {
                congratulations.gameObject.SetActive(false);
            }
        }
    }
}

