using UnityEngine;

public class InventorySlotButton : MonoBehaviour
{
    [SerializeField] private int slotIndex;

    public void OnClick()
    {
        var items = InventoryManager.Instance.items;

        if (slotIndex >= items.Count)
            return;

        ItemData item = items[slotIndex];

        if (InventoryUI.Instance.selectingOffering)
        {
            RitualManager.Instance.PlaceOffering(item);
            InventoryUI.Instance.selectingOffering = false;

            return;
        }

    }
}
