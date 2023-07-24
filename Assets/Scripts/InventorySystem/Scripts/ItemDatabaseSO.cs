using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// This scriptable object stores references to all the items in the game

[CreateAssetMenu(fileName = "AssetDatabase", menuName = "Inventory System/Item Database")]
public class ItemDatabaseSO : ScriptableObject
{    
    [System.Serializable]
    public struct ItemAsset
    {
        public string itemName;
        public ItemSO itemAsset;
    }

    // This scriptable object is a singleton
    private static ItemDatabaseSO instance;
    public static ItemDatabaseSO Instance
    {
        get
        {
            if (instance == null)
            {
                List<ItemDatabaseSO> assets = Resources.LoadAll<ItemDatabaseSO>("Databases").ToList();

                if (assets == null || assets.Count < 1) Debug.LogError("No ItemDatabase ScriptableObject found on Resources/Databases");
                else if (assets.Count > 1)
                {
                    Debug.Log("Multiple ItemDatabases found on Resources/Databases. ItemDatabase is a singleton, only one of the instances will be loaded");
                }
                else instance = assets.First();
            }

            return instance;
        }
    }

    // Unity does not serialize Dictionaries,
    // therefore we store the info in a list of structs, and then cast it to a dictionary for easier manipulation in runtime
    [field: SerializeField] List<ItemAsset> items;

    private Dictionary<string, ItemSO> itemDictionary;
    public Dictionary<string, ItemSO> ItemDictionary
    {
        get
        {
            if(itemDictionary == null)
            {
                itemDictionary = new Dictionary<string, ItemSO>();
                PopulateItemDictionary();
            }

            return itemDictionary;
        }
    }


    private void OnValidate()
    {
        LoadItemAssets();
    }

    // Load every item on an Resources/Items folder to this scriptable object
    private void LoadItemAssets()
    {
        items = new List<ItemAsset>();

        // getting a list of all the item assets that are currently stored in any Resources folder
        List<ItemSO> itemAssets = Resources.LoadAll<ItemSO>("Items").ToList();

        // Storing item names
        foreach (ItemSO itemAsset in itemAssets)
        {
            string itemName = itemAsset.name;

            if(items.Any(itemStruct => itemStruct.itemName.Equals(itemName)))
            {
                // even with repeated names, we still add it to the list -> repeated items will be removed when creating the itemDictionary
                Debug.LogError($"Multiple items with same name were found, only one of them will be acessible in runtime - item name: {itemName}");
            }

            items.Add(new ItemAsset { itemName = itemName, itemAsset = itemAsset });
        }

    }

    public bool ItemExists(string itemName)
    {
        return ItemDictionary.ContainsKey(itemName);
    }

    public bool ItemExists(ItemSO itemAsset)
    {
        return ItemDictionary.ContainsKey(itemAsset.itemName);
    }

    private void PopulateItemDictionary()
    {
        itemDictionary = new Dictionary<string, ItemSO>();

        for(int i = 0; i < items.Count; i++)
        {
            itemDictionary[items[i].itemName] = items[i].itemAsset;
        }
    }
}
