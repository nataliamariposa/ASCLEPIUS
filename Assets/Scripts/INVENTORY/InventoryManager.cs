using System.Collections.Generic;
using UnityEngine;

public enum InventoryContext
{
    Normal,
    Altar
}

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    public int maxItems = 6;
    public List<ItemData> items = new List<ItemData>();
    public ItemData selectedItem;

    public InventoryContext currentContext = InventoryContext.Normal;

    //created it as a singleton instance
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    //function to add item to inventory
    public bool AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("AddItem FAILED: item is NULL");
            return false;
        }

        if (items.Count >= maxItems)
        {
            Debug.Log("Inventory Full");
            return false;
        }

        items.Add(item);

        Debug.Log("Picked up: " + item.itemName);
        Debug.Log("New Inventory Count: " + items.Count);
        InventoryUI.Instance.RefreshUI();
        return true;
    }


    //function to remove item from inventory
    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        InventoryUI.Instance.RefreshUI();
    }

    //function to set context of inventory
    public void SetContext(InventoryContext newContext)
    {
        currentContext = newContext;
        Debug.Log("Inventory context changed to: " + currentContext);
    }

    //function that sets the selected item
    public void SelectedItem(ItemData item)
    {
        selectedItem = item;
        Debug.Log("Selected item: " + item.itemName);
    }

}
