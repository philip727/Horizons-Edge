using Philip.Inventory;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [field: SerializeField] public Inventory<Item, ItemObject.ItemGroup> Equipment { private set; get; } = new Inventory<Item, ItemObject.ItemGroup>("Equipment", 6);
    [field: SerializeField] public Inventory<Item, ItemObject.ItemGroup> Inventory { private set; get; } = new Inventory<Item, ItemObject.ItemGroup>("Backpack", 24);
}
