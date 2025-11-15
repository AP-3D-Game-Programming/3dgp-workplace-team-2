using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class UpgradeInteract : MonoBehaviour
{
    private Rigidbody interactNPC;
    public Transform player;
    public float interactRange = 3f;
    public TextMeshProUGUI interactText;
    public GameObject upgradeMenu;
    public CameraController cameraCon;
    public CarController car;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactNPC = GetComponent<Rigidbody>();
        interactText.gameObject.SetActive(false);
        upgradeMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < interactRange)
        {
            interactText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                PurchaseUp();

            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
            upgradeMenu.gameObject.SetActive(false);
            cameraCon.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    void PurchaseUp()
    {
        upgradeMenu.gameObject.SetActive(true);
        cameraCon.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void PurchaseSpeed()
    {
        if (car != null)
            car.maxAcceleration += 5f; // increase speed by 5
    }
    public void PurchaseBoost()
    {
        if (car != null)
            car.boostMultiplier += 0.2f; // increase speed by 5
    }
}
