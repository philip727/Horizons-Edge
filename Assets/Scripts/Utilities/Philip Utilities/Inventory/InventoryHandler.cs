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
            if (id >= Database.Items.Length || id < 0)
                return null;

            return Database.Items[id];
        }

    }
}
