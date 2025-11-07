using TMPro;
using UnityEngine;

public class waterDeath : MonoBehaviour
{
    public GameObject deathTextObject;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Player death");
        if (deathTextObject != null)
        {
            deathTextObject.SetActive(true);
            TextMeshProUGUI tmp = deathTextObject.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = "You ain't in a BOAT!!";
            }
        }
    }
}
