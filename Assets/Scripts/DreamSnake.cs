using System.Collections;
using UnityEngine;

public class DreamSnake : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] snakePoints;

    [Header("Player Detection")]
    public float triggerDistance = 3f;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float retreatTime = 2f;

    [Header("Appearance")]
    public float reappearDelay = 1f;

    private int currentPoint = 0;
    private Transform player;
    private bool isMoving = false;

    private Renderer[] renderers;
    private Collider[] colliders;

    public Light altarSpotlight;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;

        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        transform.position = snakePoints[0].position;
        altarSpotlight.enabled = false;

    }

    void Update()
    {
        if (player == null || isMoving)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        if (distance <= triggerDistance)
        {
            StartCoroutine(MoveToNextPoint());
        }
    }

    IEnumerator MoveToNextPoint()
    {
        isMoving = true;

        if (currentPoint + 1 >= snakePoints.Length)
        {
            DreamComplete();
            yield break;
        }

        Transform nextPoint = snakePoints[currentPoint + 1];

        // Face next destination
        Vector3 lookDirection = nextPoint.position - transform.position;

        lookDirection.y = 0;

        // Slither away into the fog

        Vector3 startPos = transform.position;
        lookDirection.y = 0;


        while (Vector3.Distance(transform.position, nextPoint.position) > 1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                nextPoint.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Hide snake
        SetVisible(false);

        // Move to next waypoint
        currentPoint++;

        transform.position =
            snakePoints[currentPoint].position;

        // Wait before reappearing
        yield return new WaitForSeconds(reappearDelay);

        // Show snake
        SetVisible(true);

        isMoving = false;
    }


    void SetVisible(bool visible)
    {
        foreach (Renderer r in renderers)
            r.enabled = visible;

        foreach (Collider c in colliders)
            c.enabled = visible;
    }

    void DreamComplete()
    {
        StartCoroutine(DreamEnding());
    }

    IEnumerator DreamEnding()
    {
        SetVisible(false);

        yield return new WaitForSeconds(1f);

        altarSpotlight.enabled = true;
        ObjectiveManager.Instance.CompleteObjective(
            ObjectiveManager.Instance.objectives[8]
        );


        ObjectiveManager.Instance.ActivateObjective(
            ObjectiveManager.Instance.objectives[9]
        );

    }
}
