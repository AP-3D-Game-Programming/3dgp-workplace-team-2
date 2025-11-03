using UnityEngine;

public class CarEntry : MonoBehaviour
{

    public Transform player;

    public Transform vehicle;

    public Transform crate;
    public CameraController CameraController;
    private bool isInVehicle = false;


    // Update is called once per frame
    void Update()
    {
        if (!isInVehicle)
        {
            if (Vector3.Distance(player.position, vehicle.position) < 3f)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    EnterVehicle();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ExitVehicle();
            }
        }
    }
    void EnterVehicle()
    {
        isInVehicle = true;

        player.GetComponent<PlayerController>().enabled = false;
        vehicle.GetComponent<CarController>().enabled = true;
        player.gameObject.SetActive(false);

        if (crate != null)
        {
            crate.GetComponent<PickupFollowFixed>().player = vehicle;
        }
        CameraController.player = vehicle;

        Debug.Log("enter vehicle complete");
    }
    void ExitVehicle()
    {
        isInVehicle = false;

        player.GetComponent<PlayerController>().enabled = true;
        vehicle.GetComponent<CarController>().enabled = false;

        player.position = transform.position + transform.right * 2f;
        player.gameObject.SetActive(true);
        if (crate != null)
        {
            crate.GetComponent<PickupFollowFixed>().player = player;
        }

        CameraController.player = player;

        Debug.Log("exited vehicle");
    }
}
