using UnityEngine;
using UnityEngine.InputSystem;

public class MenuCameraParallax : MonoBehaviour
{
    public float moveAmount = 0.2f;
    public float smoothSpeed = 3f;

    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        float mouseX = (mousePos.x / Screen.width - 0.5f) * 2f;
        float mouseY = (mousePos.y / Screen.height - 0.5f) * 2f;

        targetPos = startPos + new Vector3(
            mouseX * moveAmount,
            mouseY * moveAmount,
            0f
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}
