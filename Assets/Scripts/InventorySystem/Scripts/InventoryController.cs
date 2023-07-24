using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This class controls the inventory. It is responsible for adding, deleting and using those items
public class InventoryController : MonoBehaviour
{
    [SerializeField] InventorySO inventory;

    public void AddItem(string itemName, int numberItems = 1)
    {
        // Checking if the database contains the item
        if (!ItemDatabaseSO.Instance.ItemExists(itemName))
        {
            Debug.LogError($"Trying to add an unexisting item to the inventory. Item Name: {itemName}");
            return;
        }

        // Getting the item asset from the database
        ItemSO itemAsset = ItemDatabaseSO.Instance.ItemDictionary[itemName];

        // Checking if the item is stackable
        if (itemAsset.stackable) AddStackableItem(itemAsset, numberItems);
        else AddNonStackableItem(itemAsset, numberItems);
    }

    public void ClearInventory()
    {
        inventory.ClearInventory();
    }

    private void AddStackableItem(ItemSO itemAsset, int numberItems)
    {
        bool hasItem = inventory.HasItem(itemAsset);

        if (hasItem) // if the item is already in the inventory, add to the existing stack
        {
            int index = inventory.GetSlotIndex(itemAsset);
            ItemSlot itemSlot = inventory.items[index];

            int newNumberItems = itemSlot.numberItems += numberItems;
            if(newNumberItems < 0)
            {
                // if the operation will result in a negative quantity, delete the slot and return
                inventory.items.RemoveAt(index);
                return;
            }

            itemSlot.numberItems = newNumberItems;
            inventory.items[index] = itemSlot;
        }
        else // if the item does not exist yet, simply create a new slot for it
        {
            inventory.items.Add(new ItemSlot(itemAsset, numberItems));
        }
    }

    private void AddNonStackableItem(ItemSO itemAsset, int numberItems)
    {
        // if the number is positive, add {numberItems} slots containing the item
        if (numberItems > 0)
        {
            for (int i = 0; i < numberItems; i++) inventory.items.Add(new ItemSlot(itemAsset, 1));
        }
        else if(numberItems < 0)
        {
            numberItems *= -1;

            // if the number is negative, get all the slot indexes containing a item and delete the last {numberItems} slots
            List<int> slotIndexes = inventory.GetAllSlotIndexes(itemAsset);

            if (slotIndexes == null || slotIndexes.Count == 0) return;

            int firstIndex = slotIndexes.Count > numberItems ? slotIndexes.Count - numberItems : 0;

            for (int i = slotIndexes.Count -1; i >= firstIndex; i--)
            {
                inventory.items.RemoveAt(slotIndexes[i]);
            }
        }
    }





    private void Start()
    {
        ClearInventory();
        AddItem("teste", 5);
        AddItem("teste", -2);
    }
}
