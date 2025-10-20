using UnityEngine;
using System.Collections;

public class OlieObstacle : MonoBehaviour
{
    [SerializeField] float slideForce = 2;
    [SerializeField] float slipDuration = 1f; // Hoe lang speler geen controle heeft

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (rb != null)
            {
                // Start coroutine om te slippen
                StartCoroutine(Slip(rb, playerController));
            }
        }
    }

    private IEnumerator Slip(Rigidbody rb, PlayerController controller)
    {
        Debug.Log(" Speler glijdt!");

        // Schakel spelerbesturing uit (indien aanwezig)
        if (controller != null)
            controller.enabled = false;

        // Zet rotatie tijdelijk vast (zodat hij niet omvalt)
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Duw speler vooruit
        Vector3 slideDir = rb.transform.up;
        rb.AddForce(slideDir * slideForce, ForceMode.VelocityChange);

        // Wacht 2 seconden
        yield return new WaitForSeconds(slipDuration);

        // Herstel rotatie en controle
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // zodat hij rechtop blijft

        if (controller != null)
            controller.enabled = true;

        Debug.Log(" Speler heeft weer controle");
    }
}
