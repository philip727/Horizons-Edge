using UnityEngine;
namespace Philip.Inventory
{
    [System.Serializable]
    public class Inventory<TItem> where TItem : InventoryItem, new()
    {
        public string InventoryName { private set; get; }

        [field: SerializeField] public InventorySlot<TItem>[] Slots { protected set; get; } = new InventorySlot<TItem>[10];


        // Gets the amount of empty slots
        public int EmptySlotCount
        {
            get
            {
                int counter = 0;
                for (int i = 0; i < Slots.Length; i++)
                {
                    if (Slots[i].Item.ID <= -1)
                        counter++;
                }
                return counter;
            }
        }

        // Finds an item with the same ID
        public InventorySlot<TItem> FindItem(TItem item)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item.ID == item.ID)
                {
                    return Slots[i];
                }
            }

            return null;
        }

        // Finds the first empty slot in the inventory
        public InventorySlot<TItem> SetFirstEmptySlot(TItem item, int amount)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item.ID <= -1)
                {
                    Slots[i].UpdateSlot(item, amount);
                    return Slots[i];
                }
            }

            return null;
        }

        // Adds an item
        public bool AddItem(TItem item, int amount)
        {
            InventorySlot<TItem> inventorySlot = FindItem(item);
            if (InventoryHandler<TItem>.Instance.Database.Items[item.ID].Stackable && inventorySlot != null)
            {
                inventorySlot.AddAmount(amount);
                return true;
            }

            if (EmptySlotCount <= 0)
                return false;

            if (!InventoryHandler<TItem>.Instance.Database.Items[item.ID].Stackable || inventorySlot == null)
            {
                SetFirstEmptySlot(item, amount);
                return true;
            }

            return false;
        }
    }
}