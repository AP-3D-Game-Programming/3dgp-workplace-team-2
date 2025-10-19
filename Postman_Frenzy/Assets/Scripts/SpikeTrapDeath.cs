using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpikeTrapDeath : MonoBehaviour
{
    public Vector3 respawnPosition = Vector3.zero;
    public Text deathText;
    public float deathMessageDuration = 2f;
    public string deadlyTag = "Deadly";

    // If set in Inspector, this will be displayed on death.
    // If left empty, the Text component's current value (set in the Editor) will be used.
    public string deathMessage = "";

    // cached default text taken from the Text component at Start
    string defaultText;

    void Start()
    {
        if (respawnPosition == Vector3.zero)
            respawnPosition = transform.position;

        if (deathText == null) return;

        // store the editor value so we can fall back to it
        defaultText = deathText.text;

        var canvas = deathText.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            canvas.gameObject.SetActive(true);
        }

        var rt = deathText.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(800f, 200f);
        rt.localScale = Vector3.one;

        deathText.alignment = TextAnchor.MiddleCenter;
        deathText.fontSize = 72;
        deathText.horizontalOverflow = HorizontalWrapMode.Overflow;
        deathText.verticalOverflow = VerticalWrapMode.Overflow;

        deathText.canvasRenderer.SetAlpha(1f);

        var groups = deathText.GetComponentsInParent<CanvasGroup>(true);
        foreach (var g in groups)
        {
            g.alpha = 1f;
            g.interactable = true;
            g.blocksRaycasts = true;
            g.gameObject.SetActive(true);
        }

        deathText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.tag == deadlyTag)
            StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        if (deathText != null)
        {
            // use override if provided, otherwise use the Text component's editor value
            deathText.text = string.IsNullOrEmpty(deathMessage) ? defaultText : deathMessage;

            deathText.alignment = TextAnchor.MiddleCenter;
            deathText.fontSize = 72;

            var rt = deathText.rectTransform;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(800f, 200f);
            rt.localScale = Vector3.one;

            deathText.horizontalOverflow = HorizontalWrapMode.Overflow;
            deathText.verticalOverflow = VerticalWrapMode.Overflow;

            deathText.canvasRenderer.SetAlpha(1f);
            deathText.gameObject.SetActive(true);
            deathText.transform.SetAsLastSibling();
        }

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = respawnPosition;

        yield return new WaitForSeconds(deathMessageDuration);

        if (deathText != null)
            deathText.gameObject.SetActive(false);
    }
}
