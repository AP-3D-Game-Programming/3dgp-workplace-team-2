using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SelectDeliveryHouse : MonoBehaviour
{
    public Transform crateParent;
    public Transform houseParent;
    public Transform player;             // the player transform
    public float deliveryRange = 10f;    // how close you must be to deliver
    public Color highlightColor = Color.green;  // new color to indicate selection
    public TextMeshProUGUI deliveryText;
    public MoneyEarnspend moneyUpdate;

    private List<CrateHoldScript> crates = new List<CrateHoldScript>();
    private Transform selectedHouse;
    private Material originalMaterial;
    private bool houseSelected = false;

    void Update()
    {
        RefreshCrateList();

        bool anyHeld = false;
        CrateHoldScript heldCrate = null;

        foreach (var crate in crates)
        {
            if (crate != null && crate.isHeld)
            {
                anyHeld = true;
                heldCrate = crate;
                break;
            }
        }

        if (!houseSelected && anyHeld)
        {
            SelectRandomHouse();
            houseSelected = true;
        }

        if (houseSelected && selectedHouse != null)
        {
            float dist = Vector3.Distance(player.position, selectedHouse.position);

            if (anyHeld && dist <= deliveryRange)
            {
                deliveryText.text = "Press 'E' to deliver package";
                deliveryText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    TryDeliverCrate(heldCrate);
                }
            }
            else
            {
                deliveryText.gameObject.SetActive(false);
            }
        }
    }

    void RefreshCrateList()
    {
        if (crateParent == null) return;

        crates.Clear();
        foreach (Transform child in crateParent)
        {
            var crateScript = child.GetComponent<CrateHoldScript>();
            if (crateScript != null)
            {
                crates.Add(crateScript);
            }
        }
    }

    void SelectRandomHouse()
    {
        if (houseParent == null || houseParent.childCount == 0)
        {
            Debug.LogWarning("No houses found under the specified parent!");
            return;
        }

        int randomIndex = Random.Range(0, houseParent.childCount);
        selectedHouse = houseParent.GetChild(randomIndex);

        Renderer rend = selectedHouse.GetComponent<Renderer>();
        if (rend != null)
        {
            // Save the original material
            originalMaterial = rend.material;

            // Create a new material so we can change its color safely
            Material colorMat = new Material(originalMaterial);
            colorMat.color = highlightColor;  // just change the base color
            rend.material = colorMat;
        }

        Debug.Log("Selected delivery house: " + selectedHouse.name);
    }

    void TryDeliverCrate(CrateHoldScript heldCrate)
    {
        if (selectedHouse == null || heldCrate == null) return;

        float dist = Vector3.Distance(player.position, selectedHouse.position);
        Debug.Log(dist + " from house");

        if (dist <= deliveryRange)
        {
            Debug.Log("Crate delivered to: " + selectedHouse.name);

            ResetHouseColor();

            heldCrate.isHeld = false;
            if (heldCrate.TryGetComponent<Rigidbody>(out var rb))
                rb.isKinematic = false;

            deliveryText.gameObject.SetActive(false);
            Destroy(heldCrate.gameObject);

            houseSelected = false;
            moneyUpdate.AddMoney(100);
        }
        else
        {
            Debug.Log("Too far from the delivery house!");
        }
    }

    void ResetHouseColor()
    {
        if (selectedHouse == null) return;

        Renderer rend = selectedHouse.GetComponent<Renderer>();
        if (rend != null && originalMaterial != null)
        {
            rend.material = originalMaterial;
        }

        selectedHouse = null;
        originalMaterial = null;
    }
}
