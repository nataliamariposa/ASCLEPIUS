using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JournalNotification : MonoBehaviour
{
    public static JournalNotification Instance;

    [Header("References")]
    public Image journalIcon;
    public GameObject unreadDot;

    [Header("Pulse Settings")]
    public int pulseCount = 3;
    public float pulseDuration = 0.4f;
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;

    private bool hasUnread = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (unreadDot != null)
            unreadDot.SetActive(false);

        Color c = journalIcon.color;
        c.a = maxAlpha;
        journalIcon.color = c;
    }

    public void NotifyNewFact()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pencilScratch);

        StopAllCoroutines();
        StartCoroutine(PulseIcon());

        hasUnread = true;
        if (unreadDot != null)
            unreadDot.SetActive(true);
    }

    public void ClearUnread()
    {
        hasUnread = false;
        if (unreadDot != null)
            unreadDot.SetActive(false);
    }

    private IEnumerator PulseIcon()
    {
        for (int i = 0; i < pulseCount; i++)
        {
            // fade out
            yield return FadeIcon(maxAlpha, minAlpha, pulseDuration * 0.5f);
            // fade in
            yield return FadeIcon(minAlpha, maxAlpha, pulseDuration * 0.5f);
        }

        // settle at full alpha
        Color c = journalIcon.color;
        c.a = maxAlpha;
        journalIcon.color = c;
    }

    private IEnumerator FadeIcon(float from, float to, float duration)
    {
        float t = 0f;
        Color c = journalIcon.color;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            journalIcon.color = c;
            yield return null;
        }
        c.a = to;
        journalIcon.color = c;
    }
}