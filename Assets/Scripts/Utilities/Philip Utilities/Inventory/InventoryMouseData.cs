using UnityEngine;

namespace Philip.Inventory
{
    public static class InventoryMouseData<TItem, TItemGroup> where TItem : InventoryItem, new() where TItemGroup : System.Enum
    {
        public static InventoryInterface<TItem, TItemGroup> s_interfaceMouseIsOver;
        public static GameObject s_TempItemBeingDragged;
        public static GameObject s_slotHoveredOver;
        public static GameObject s_slotSelected;
    }
}
