using UnityEngine;
using StarterAssets;

public class FootstepController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] stoneFootsteps;
    public AudioClip[] dirtFootsteps;

    [Header("Settings")]
    public float stepInterval = 0.5f; // time between steps
    public float velocityThreshold = 0.1f; // minimum speed to trigger steps

    [Header("Surface Detection")]
    public float raycastDistance = 1.5f;
    public LayerMask groundLayer;

    private float stepTimer = 0f;
    private CharacterController controller;
    private int lastStoneIndex = -1;
    private int lastDirtIndex = -1;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsMoving()) return;
        if (!IsGrounded()) return;

        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = stepInterval;
        }
    }

    private bool IsMoving()
    {
        Vector3 horizontalVelocity = new Vector3(
            controller.velocity.x, 0f, controller.velocity.z
        );
        return horizontalVelocity.magnitude > velocityThreshold;
    }

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void PlayFootstep()
    {
        AudioClip clip = GetFootstepClip();
        if (clip != null)
            AudioManager.Instance.PlaySFX(clip);
    }

    private AudioClip GetFootstepClip()
    {
        // raycast down to detect surface
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
        {
            Debug.Log("Hit: " + hit.collider.name + " Tag: " + hit.collider.tag);

            if (hit.collider.CompareTag("Dirt"))
                return GetRandomClip(dirtFootsteps, ref lastDirtIndex);
            else
                return GetRandomClip(stoneFootsteps, ref lastStoneIndex);
        }

        // default to stone if nothing detected
        return GetRandomClip(stoneFootsteps, ref lastStoneIndex);
    }

    // picks a random clip without repeating the last one
    private AudioClip GetRandomClip(AudioClip[] clips, ref int lastIndex)
    {
        if (clips == null || clips.Length == 0) return null;
        if (clips.Length == 1) return clips[0];

        int index;
        do
        {
            index = Random.Range(0, clips.Length);
        } while (index == lastIndex);

        lastIndex = index;
        return clips[index];
    }
}