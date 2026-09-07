using UnityEngine;

public class PersistentPickup : MonoBehaviour
{
    [SerializeField] private string itemName;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.WasItemCollected(itemName))
            gameObject.SetActive(false);
    }
}