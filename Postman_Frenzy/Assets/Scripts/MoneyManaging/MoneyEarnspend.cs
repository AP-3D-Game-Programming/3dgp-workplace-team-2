using UnityEngine;
using TMPro;
public class MoneyEarnspend : MonoBehaviour
{
    public static MoneyEarnspend Instance;
    public int money = 0;

    public TextMeshProUGUI moneyText; // assign in inspector

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyUI();
            return true;
        }
        return false;
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = money.ToString() + " $"; // $ after the number
    }
}
