using Philip.Utilities;

namespace Philip.Inventory
{
    // Only one of these should be placed
    public class InventoryHandler<TItem> : Singleton<InventoryHandler<TItem>> where TItem : InventoryItem, new()
    {
        public InventoryItemDatabase<TItem> Database;
    }
}
