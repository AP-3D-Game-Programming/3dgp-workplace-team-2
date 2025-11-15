using UnityEngine;
using TMPro;
using System.Collections;

public class WaterDeath : MonoBehaviour
{
    public Transform water;
    public Transform player;
    public Transform van;
    public TextMeshProUGUI waterDeathtxt;
    public float respawnDelay = 1.5f; // how long the text stays visible

    private Vector3 startPosPlayer;
    private Quaternion startRotPlayer;
    private Vector3 startPosVan;
    private Quaternion startRotVan;
    private bool isRespawning = false;

    void Start()
    {
        waterDeathtxt.gameObject.SetActive(false);
        startPosPlayer = player.position;
        startRotPlayer = player.rotation;
        startPosVan = van.position;
        startRotVan = van.rotation;
    }

    void Update()
    {
        if (!isRespawning && player.position.y < water.position.y - 2)
        {
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {
        isRespawning = true;

        waterDeathtxt.gameObject.SetActive(true);

        yield return new WaitForSeconds(respawnDelay);

        waterDeathtxt.gameObject.SetActive(false);

        player.position = startPosPlayer;
        player.rotation = startRotPlayer;
        van.position = startPosVan;
        van.rotation = startRotVan;


        Rigidbody rbPlayer = player.GetComponent<Rigidbody>();
        Rigidbody rbVan = van.GetComponent<Rigidbody>();
        if (rbVan != null && rbPlayer != null)
        {
            rbPlayer.linearVelocity = Vector3.zero;
            rbPlayer.angularVelocity = Vector3.zero;
            rbVan.linearVelocity = Vector3.zero;
            rbVan.angularVelocity = Vector3.zero;
        }

        isRespawning = false;
    }
}
