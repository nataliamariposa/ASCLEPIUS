using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    public int slotIndex;

    public void OnClick()
    {
        Debug.Log("SLOT CLICKED");

        InventoryUI.Instance.SelectItem(slotIndex);
        var item = InventoryManager.Instance.items[slotIndex];
        InventoryManager.Instance.SelectedItem(item);

    }
}
