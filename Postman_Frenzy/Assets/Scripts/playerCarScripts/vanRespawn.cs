using UnityEngine;

public class VanRespawner : MonoBehaviour
{
    [SerializeField] private GameObject van;         // Reference to your van
    [SerializeField] private Transform player;   // Start position of the van

    private Vector3 spawn = new Vector3(96.25f, 104.74f, 106.02f);

    private Rigidbody vanRb;

    void Start()
    {
        if (van == null)
        {
            Debug.LogError("Van reference is missing!");
            return;
        }

        vanRb = van.GetComponent<Rigidbody>();
        if (vanRb == null)
        {
            Debug.LogWarning("Van has no Rigidbody. Velocity reset will be skipped.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnVan();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            respawnPlayer();
        }
    }

    void RespawnVan()
    {
        if (vanRb != null)
        {
            vanRb.linearVelocity = Vector3.zero;
            vanRb.angularVelocity = Vector3.zero;
        }

        if (player != null)
            van.transform.position = player.position - player.right * 3;
        else
            van.transform.position = spawn; // pointfallback if no start 

        vanRb.linearVelocity = Vector3.zero;
        vanRb.angularVelocity = Vector3.zero;

        van.transform.rotation = Quaternion.identity; // optional: reset rotation
    }
    void respawnPlayer()
    {
        van.transform.position = spawn;
        player.transform.position = spawn;
    }
}
