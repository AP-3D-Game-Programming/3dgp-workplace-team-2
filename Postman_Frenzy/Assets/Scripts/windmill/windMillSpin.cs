using UnityEngine;

public class WindmillController : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public Transform blades;

    void Update()
    {
        if (blades != null)
            blades.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}