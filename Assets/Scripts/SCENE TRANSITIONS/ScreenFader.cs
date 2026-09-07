using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    public bool IsFading { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Start fully black so FadeIn in Start() can clear it
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void ResetToBlack()
    {
        StopAllCoroutines(); // Kill any in-progress fade first
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(canvasGroup.alpha, 1f); // Fade FROM current alpha, not hardcoded 0
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(canvasGroup.alpha, 0f); // Fade FROM current alpha, not hardcoded 1
    }

    public IEnumerator FadeToScene(string sceneName)
    {
        if (AudioManager.Instance != null)
        {
        StartCoroutine(AudioManager.Instance.FadeOutAmbience(fadeDuration));
        }      

        Debug.Log("FadeToScene: starting FadeOut");
        yield return FadeOut();
        Debug.Log("FadeToScene: loading scene");
        SceneManager.LoadScene(sceneName);
        yield return null;
        yield return null;
        Debug.Log("FadeToScene: setting black, about to FadeIn");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        Debug.Log("FadeToScene: calling FadeIn");
        yield return FadeIn();
        Debug.Log("FadeToScene: FadeIn complete");
    }

    private IEnumerator Fade(float start, float end)
    {
        IsFading = true;
        float t = 0f;
        canvasGroup.alpha = start;
        canvasGroup.blocksRaycasts = true;
        yield return null; // skip the spike frame before starting to accumulate time


        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = end;
        IsFading = false;
        // Only unblock raycasts when fully transparent
        if (end == 0f)
            canvasGroup.blocksRaycasts = false;
    }
}