using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemSlot
{
    public ItemSO itemAsset;
    public int numberItems;

    public ItemSlot(ItemSO itemAsset, int numberItems)
    {
        this.itemAsset = itemAsset;
        this.numberItems = numberItems;
    }
}

// This scritable object store the references (and quantities) of all the items stored on a given inventory
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{
    [field: SerializeField] public List<ItemSlot> items = new List<ItemSlot>();

    public bool HasItem(string itemName)
    {
        return items.Any(slot => slot.itemAsset.itemName == itemName);
    }

    public bool HasItem(ItemSO itemAsset)
    {
        return items.Any(slot => slot.itemAsset == itemAsset);
    }

    // Returns the index of a slot that contains a given item. Returns -1 if the inventory does not contain the item
    public int GetSlotIndex(ItemSO itemAsset)
    {
        if (!items.Any(slot => slot.itemAsset == itemAsset)) return -1;
        return items.FindIndex(slot => slot.itemAsset == itemAsset);
    }

    // Returns a list containing the indexes of all the slots with a given item. Returns null if the inventory does not contain the item
    public List<int> GetAllSlotIndexes(ItemSO itemAsset)
    {
        if (!items.Any(slot => slot.itemAsset == itemAsset)) return null;
        return Enumerable.Range(0, items.Count)
            .Where(i => items[i].itemAsset == itemAsset)
            .ToList();
    }

    public void ClearInventory()
    {
        items = new List<ItemSlot>();
    }
}
