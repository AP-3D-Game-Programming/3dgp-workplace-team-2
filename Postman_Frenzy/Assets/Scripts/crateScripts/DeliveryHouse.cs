using UnityEngine;

public class DeliveryHouse : MonoBehaviour
{
    [HideInInspector] public bool isTarget = false;

    private Renderer rend;
    private Color defaultColor;
    public Color targetColor = Color.green;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            defaultColor = rend.material.color;
    }

    void Update()
    {
        if (rend != null)
            rend.material.color = isTarget ? targetColor : defaultColor;
    }

    public void ResetHouse()
    {
        isTarget = false;
        if (rend != null)
            rend.material.color = defaultColor;
    }
}
