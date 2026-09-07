using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea]
    public string description;
    public GameObject worldPrefab; 
    public Vector3 spawnRotationOffset;
    public int offeringValue = 1;
}
