using Philip.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemObject;

[CreateAssetMenu(fileName = "New Object", menuName = "Philip/Inventory/Item Object")]
public class ItemObject : InventoryItemObject<Item, ItemGroup>
{
    public enum ItemGroup
    {
        Material,
        Pickaxe,
        Axe,
        Sword,
        Bow,
        Staff
    }
}