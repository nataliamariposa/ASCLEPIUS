using UnityEngine;

public class PulseLight : MonoBehaviour
{
    public Light glowLight;

    public float minIntensity = 5f;
    public float maxIntensity = 20f;
    public float pulseSpeed = 2f;

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        glowLight.intensity = Mathf.Lerp(
            minIntensity,
            maxIntensity,
            t
        );
    }
}
