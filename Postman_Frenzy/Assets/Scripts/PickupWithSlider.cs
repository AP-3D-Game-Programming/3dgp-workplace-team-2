using UnityEngine;
using UnityEngine.UI;

public class PickupWithSlider : MonoBehaviour
{
    [Header("Timer instellingen")]
    public float timerDuration = 10f;
    private float timer;
    private bool timerRunning = false;

    [Header("UI")]
    public Slider timerSlider;

    private PickupPrompt prompt;
    private PickupFollowFixed pickupFollow;

    void Start()
    {
        timer = timerDuration;

        prompt = GetComponent<PickupPrompt>(); // zoek de prompt op hetzelfde object
        pickupFollow = GetComponent<PickupFollowFixed>(); // get reference to pickup script

        if (timerSlider != null)
        {
            timerSlider.maxValue = timerDuration;
            timerSlider.value = timerDuration;
            timerSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (timerRunning)
        {
            timer -= Time.deltaTime;

            if (timerSlider != null)
                timerSlider.value = timer;

            if (timer <= 0f)
            {
                timerRunning = false;

                // Reset the target house if one was assigned
                if (pickupFollow != null && pickupFollow.targetHouse != null)
                {
                    pickupFollow.targetHouse.ResetHouse();
                }

                // Zet prompt uit zodat er geen tekst meer verschijnt
                if (prompt != null)
                {
                    prompt.isHeld = false;
                    if (prompt.promptText != null)
                        prompt.promptText.gameObject.SetActive(false);
                }

                Destroy(gameObject); // object verdwijnt
            }
        }
    }

    public void StartTimer()
    {
        if (!timerRunning)
        {
            timer = timerDuration;
            timerRunning = true;

            if (timerSlider != null)
            {
                timerSlider.value = timerDuration;
                timerSlider.gameObject.SetActive(true);
            }
        }
    }
}
