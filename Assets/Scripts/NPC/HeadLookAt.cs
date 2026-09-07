using UnityEngine;

public class HeadLookAt : MonoBehaviour
{

    public Transform player;

    void Update()
    {
        transform.position = player.position + Vector3.up * 2f;
    }
}
