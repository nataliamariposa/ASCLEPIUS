using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class TempleZoneTrigger : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Reverb Settings")]
    public float indoorRoomValue = -1000f;
    public float outdoorRoomValue = -10000f;
    public float transitionDuration = 1f;

    private bool isInside = false;
    private bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isInside || isTransitioning) return;
        isInside = true;
        StartCoroutine(TransitionToIndoor());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!isInside || isTransitioning) return;
        isInside = false;
        StartCoroutine(TransitionToOutdoor());
    }

    private IEnumerator TransitionToIndoor()
    {
        isTransitioning = true;
        AudioManager.Instance.StartCoroutine(
            AudioManager.Instance.CrossfadeAmbience(
                AudioManager.Instance.templeAtmosphere,
                transitionDuration
            )
        );
        yield return StartCoroutine(
            TransitionReverb(indoorRoomValue, transitionDuration)
        );
        isTransitioning = false;
    }

    private IEnumerator TransitionToOutdoor()
    {
        isTransitioning = true;
        AudioManager.Instance.StartCoroutine(
            AudioManager.Instance.CrossfadeAmbience(
                AudioManager.Instance.nightAmbience,
                transitionDuration
            )
        );
        yield return StartCoroutine(
            TransitionReverb(outdoorRoomValue, transitionDuration)
        );
        isTransitioning = false;
    }

    private IEnumerator TransitionReverb(float targetValue, float duration)
    {
        float currentValue;
        audioMixer.GetFloat("FootstepsReverb", out currentValue);
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            audioMixer.SetFloat("FootstepsReverb",
                Mathf.Lerp(currentValue, targetValue, t / duration));
            yield return null;
        }
        audioMixer.SetFloat("FootstepsReverb", targetValue);
    }
}