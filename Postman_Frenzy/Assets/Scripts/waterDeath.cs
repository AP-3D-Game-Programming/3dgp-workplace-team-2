using UnityEngine;
using TMPro;

public class waterDeath : MonoBehaviour
{
    public Transform water;
    public Transform player;
    public TextMeshProUGUI waterDeathtxt;

    void Start()
    {
        waterDeathtxt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player.position.y < water.position.y - 2)
        {
            waterDeathtxt.gameObject.SetActive(true);
        }
        else
        {
            waterDeathtxt.gameObject.SetActive(false);
        }
    }
}
