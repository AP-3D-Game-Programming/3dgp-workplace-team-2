using UnityEngine;
using TMPro;

public class UpgradeInteract : MonoBehaviour
{
    private Rigidbody interactNPC;
    public Transform player;
    public float interactRange = 3f;
    public TextMeshProUGUI interactText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactNPC = GetComponent<Rigidbody>();
        interactText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < interactRange)
        {
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }
}
