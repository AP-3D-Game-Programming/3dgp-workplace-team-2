using UnityEngine;
using TMPro;

public class UpgradeInteract : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public float interactRange = 3f;
    public TextMeshProUGUI interactText;
    public GameObject upgradeMenu;
    public CameraController cameraCon;
    public CarController car;
    public MoneyEarnspend moneyUpdate;

    private bool upgradeUIOpen = false;
    private Rigidbody interactNPC;

    void Start()
    {
        interactNPC = GetComponent<Rigidbody>();
        interactText.gameObject.SetActive(false);
        upgradeMenu.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // Show interact text when player is in range
        if (distance < interactRange)
        {
            interactText.gameObject.SetActive(true);

            // Toggle upgrade menu with E
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleUpgradeMenu();
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);

            if (upgradeUIOpen)
            {
                CloseUpgradeMenu();
            }
        }
    }

    void ToggleUpgradeMenu()
    {
        if (upgradeUIOpen)
            CloseUpgradeMenu();
        else
            OpenUpgradeMenu();
    }

    void OpenUpgradeMenu()
    {
        upgradeUIOpen = true;
        upgradeMenu.SetActive(true);
        cameraCon.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseUpgradeMenu()
    {
        upgradeUIOpen = false;
        upgradeMenu.SetActive(false);
        cameraCon.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Upgrade buttons
    public void PurchaseSpeed()
    {
        if (moneyUpdate.SpendMoney(10))
        {
            if (car != null)
                car.maxSpeed += 5f;
        }
        else
        {
            Debug.Log("Not enough money to purchase speed!");
        }
    }

    public void PurchaseBoost()
    {
        if (moneyUpdate.SpendMoney(10))
        {
            if (car != null)
                car.boostMultiplier += 0.2f;
        }
        else
        {
            Debug.Log("Not enough money to purchase boost!");
        }
    }

    public void PurchaseBoostDur()
    {
        if (moneyUpdate.SpendMoney(10))
        {
            if (car != null)
                car.boostDuration += 0.5f;
        }
        else
        {
            Debug.Log("Not enough money to purchase boost duration!");
        }
    }
}
