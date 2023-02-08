using Philip.Utilities;

namespace Philip.Inventory
{
    // Only one of these should be placed
    [System.Serializable]
    public class InventoryHandler<TItem, TItemType> where TItem : InventoryItem, new() where TItemType : System.Enum
    {
        public InventoryItemDatabase<TItem, TItemType> Database;

        public InventoryItemObject<TItem, TItemType> GetItem(int id)
        {
            return Database.Items[id];
        }

    }
}
