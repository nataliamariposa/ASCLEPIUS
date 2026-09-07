using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData itemData;

    public void Interact()
    {
        if (itemData == null)
        {
            Debug.LogError("WorldItem has NO itemData assigned on " + gameObject.name);
            return;
        }

        bool added = InventoryManager.Instance.AddItem(itemData);

        if (added)
        {
            AudioManager.Instance.PlayItemPickup();
            ObjectiveManager.Instance.CompleteObjective(ObjectiveManager.Instance.objectives[4]);
            ObjectiveManager.Instance.NotifyKeepsakeCompleted();
            GameManager.Instance.RecordItemCollected(itemData.itemName); // add this
            Debug.Log("Item added! WorldItem script working");
            Destroy(gameObject);
            Debug.Log("gameObject destroyed! ");
        }
    }

}
