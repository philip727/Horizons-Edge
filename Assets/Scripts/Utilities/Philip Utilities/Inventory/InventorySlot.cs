using System;
using UnityEngine;

namespace Philip.Inventory
{
    [System.Serializable]
    public class InventorySlot<TItem, TItemType> where TItem : InventoryItem, new() where TItemType : Enum
    {
        public delegate void InvetorySlotUpdate(InventorySlot<TItem, TItemType> _inventorySlot);

        [field: SerializeField] public TItemType[] AllowedItemTypes = new TItemType[0];
        [field:SerializeField] public TItem Item { private set; get; }
        [field: SerializeField] public long Amount { private set; get; }



        [System.NonSerialized] public InvetorySlotUpdate OnBeforeUpdate;
        [System.NonSerialized] public InvetorySlotUpdate OnAfterUpdate;

        // Default inventory slot with -1 ID so we can display it properly
        public InventorySlot()
        {
            UpdateSlot(new TItem(), 0);
        }

        // Creates inventory slot with item
        public InventorySlot(TItem item, int amount)
        {
            UpdateSlot(item, amount);
        }

        // Updates the inventory slot
        public void UpdateSlot(TItem item, long amount)
        {
            OnBeforeUpdate?.Invoke(this);
            Item = item;
            Amount = amount;
            OnAfterUpdate?.Invoke(this);
        }

        public void RemoveItem()
        {
            UpdateSlot(new TItem(), 0);
        }

        public void AddAmount(int value)
        {
            UpdateSlot(Item, Amount += value);
        }
    }
}
