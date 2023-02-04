using Philip.Utilities;

namespace Philip.Inventory
{
    // Only one of these should be placed
    public class InventoryHandler<TItem, TItemType> : Singleton<InventoryHandler<TItem, TItemType>> where TItem : InventoryItem, new() where TItemType : System.Enum
    {
        public InventoryItemDatabase<TItem, TItemType> Database;
    }
}
