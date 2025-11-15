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
    public MoneyEarnspend moneyUpdate;
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
        if (moneyUpdate.SpendMoney(10))
        {
            if (car != null)
                car.maxSpeed += 10f;
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
            Debug.Log("Not enough money to purchase speed!");
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
            Debug.Log("Not enough money to purchase speed!");
        }
    }
}
