using Philip.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Database", menuName = "Philip/Inventory/Database")]
public class ItemDatabase : InventoryItemDatabase<Item, ItemObject.ItemGroup>
{
    [ContextMenu("Update ID's")]
    public override void UpdateIDs()
    {
        base.UpdateIDs();
    }
}
