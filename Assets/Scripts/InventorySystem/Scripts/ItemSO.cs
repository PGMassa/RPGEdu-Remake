using UnityEngine;

// This scriptable object stores all the invormation about a given item

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string itemName;
    [field: SerializeField] public Sprite sprite;
    [field: SerializeField] public bool stackable = false;

    [TextArea(20, 15)]
    [field: SerializeField] public string description;

    private void OnValidate()
    {
        this.itemName = this.name;
    }
}
