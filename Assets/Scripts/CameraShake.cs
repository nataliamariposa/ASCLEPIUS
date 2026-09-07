using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float intensity = 0.05f;
    public bool shaking = false;

    private Vector3 originalPos;

    private void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (shaking)
        {
            transform.localPosition =
                originalPos + Random.insideUnitSphere * intensity;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void StartShake()
    {
        shaking = true;
    }

    public void StopShake()
    {
        shaking = false;
    }
}
