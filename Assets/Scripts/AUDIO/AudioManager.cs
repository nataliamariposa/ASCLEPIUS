using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambienceSource;

    [Header("UI Sounds")]
    public AudioClip buttonClick;
    public AudioClip journalOpen;
    public AudioClip journalClose;
    public AudioClip itemPickup;

    [Header("Ambience")]
    public AudioClip nightAmbience;
    public AudioClip templeAtmosphere;
    public AudioClip mainTheme;
    public AudioClip dreamAmbience;

    [Header("Journal")]
    public AudioClip pencilScratch;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Objectives")]
    public AudioClip newObjectiveSound;

    [Header("Volume")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float ambienceVolume = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // load saved volumes
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", 0.5f);

        sfxSource.volume = sfxVolume;
        ambienceSource.volume = ambienceVolume;
        if (audioMixer != null)
        {
        audioMixer.SetFloat("FootstepsReverb", -10000f);
        }
    }

    // -------------------------
    // SFX
    // -------------------------
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayButtonClick() => PlaySFX(buttonClick);
    public void PlayJournalOpen() => PlaySFX(journalOpen);
    public void PlayJournalClose() => PlaySFX(journalClose);
    public void PlayItemPickup() => PlaySFX(itemPickup);

    // -------------------------
    // AMBIENCE
    // -------------------------
    public void PlayAmbience(AudioClip clip, bool forceRestart = false)
    {
        if (clip == null) return;
        if (ambienceSource.clip == clip && ambienceSource.isPlaying && !forceRestart) return;
        ambienceSource.clip = clip;
        ambienceSource.loop = true;
        ambienceSource.volume = ambienceVolume;
        ambienceSource.Play();
    }

    public void StopAmbience()
    {
        ambienceSource.Stop();
    }

    public void PlayNightAmbience() => PlayAmbience(nightAmbience);
    public void PlayTempleAmbience() => PlayAmbience(templeAtmosphere);
    public void PlayMainAmbience() => PlayAmbience(mainTheme);
    public void PlayDreamAmbience() => PlayAmbience(dreamAmbience);

    public IEnumerator CrossfadeAmbience(AudioClip newClip, float duration = 1f)
    {
        // fade out current
        float startVolume = ambienceSource.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            ambienceSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        ambienceSource.Stop();
        ambienceSource.clip = newClip;
        ambienceSource.loop = true;
        ambienceSource.Play();

        // fade in new
        t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            ambienceSource.volume = Mathf.Lerp(0f, ambienceVolume, t / duration);
            yield return null;
        }

        ambienceSource.volume = ambienceVolume;
    }

    public IEnumerator FadeOutAmbience(float duration = 1f)
    {
        float startVolume = ambienceSource.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            ambienceSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        ambienceSource.Stop();
        ambienceSource.volume = ambienceVolume; // reset for next scene
    }

    // -------------------------
    // VOLUME CONTROL
    // -------------------------
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public void SetAmbienceVolume(float value)
    {
        ambienceVolume = value;
        ambienceSource.volume = value;
        PlayerPrefs.SetFloat("AmbienceVolume", value);
        PlayerPrefs.Save();
    }
}
