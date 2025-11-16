using UnityEngine;
using TMPro;

public class CrateHoldScript : MonoBehaviour
{
    public Transform player;
    public float interactRange = 3f;
    public TextMeshProUGUI interactText;
    public Vector3 holdOffset = new Vector3(0, 0.5f, 2f);
    public Vector3 holdRotation = new Vector3(0, 0, 0);
    public float dropDistance = -1f;

    private Rigidbody crate;
    private BoxCollider crateCollider;
    public bool isHeld = false;

    void Start()
    {
        crate = GetComponent<Rigidbody>();
        crateCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(player.position, transform.position);
        if (!isHeld)
        {
            interactText.gameObject.SetActive(playerDistance < interactRange);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }

        if (playerDistance < interactRange && Input.GetKeyDown(KeyCode.E))
        {
            isHeld = !isHeld;

            if (isHeld)
            {
                crate.isKinematic = true;
                crateCollider.enabled = false;
            }
            else
            {
                crate.isKinematic = false;
                crateCollider.enabled = true;

                RaycastHit hit;
                Vector3 dropPos = player.position + player.forward * holdOffset.z + Vector3.up * holdOffset.y;

                if (Physics.Raycast(player.position + Vector3.up * holdOffset.y, player.forward, out hit, holdOffset.z + dropDistance))
                {
                    dropPos = hit.point - player.forward * 0.5f;
                }

                transform.position = dropPos;
                transform.rotation = player.rotation * Quaternion.Euler(holdRotation);

                crate.linearVelocity = Vector3.zero;
                crate.angularVelocity = Vector3.zero;
            }
        }

        if (isHeld)
        {
            transform.position = player.position + player.forward * holdOffset.z + Vector3.up * holdOffset.y;
            transform.rotation = player.rotation * Quaternion.Euler(holdRotation);
        }
    }

    public bool isLoadedInVan = false;


    public void TryLoadOrUnload()
    {
        if (isHeld)
        {
            isHeld = false;
            isLoadedInVan = true;
            crate.isKinematic = true;
            crateCollider.enabled = false;

            transform.position = new Vector3(1000, 1000, 1000);
            Debug.Log("Crate loaded into van!");
        }
        else if (isLoadedInVan)
        {
            // Unload from van
            isLoadedInVan = false;
            crate.isKinematic = false;
            crateCollider.enabled = true;

            transform.position = player.position + player.forward * 1.5f;
            Debug.Log("Crate unloaded from van!");
        }
    }
}
