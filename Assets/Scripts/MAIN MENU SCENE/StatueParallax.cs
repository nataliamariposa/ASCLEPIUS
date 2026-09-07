using UnityEngine;
using UnityEngine.InputSystem;


public class StatueParallax : MonoBehaviour
{
    public float amount = 0.05f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        float mouseX = (mousePos.x / Screen.width - 0.5f);
        float mouseY = (mousePos.y / Screen.height - 0.5f);

        transform.position = startPos + new Vector3(
            mouseX * amount,
            mouseY * amount,
            0
        );
    }
}