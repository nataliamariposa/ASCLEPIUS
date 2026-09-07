using UnityEngine;

public class UseButton : MonoBehaviour
{
    public void UseSelectedItem()
    {
        var inventory = InventoryManager.Instance;
        var ritual = RitualManager.Instance;

        if (inventory.selectedItem == null)
        {
            Debug.Log("No item selected");
            return;
        }

        if (inventory.currentContext == InventoryContext.Normal)
        {
            Debug.Log("Use does nothing in normal inventory");
            return;
        }

        if (inventory.currentContext == InventoryContext.Altar)
        {
            Debug.Log("Using item at altar: " + inventory.selectedItem.itemName);

            ritual.PlaceOffering(inventory.selectedItem);

            // clear selection after use
            inventory.selectedItem = null;
        }
    }
}
