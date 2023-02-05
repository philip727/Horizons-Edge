using UnityEngine;
using System;
namespace Philip.Inventory
{
    [System.Serializable]
    public class Inventory<TItem, TItemType> where TItem : InventoryItem, new() where TItemType : Enum
    {
        public string InventoryName { private set; get; }
        [field: SerializeField] public InventorySlot<TItem, TItemType>[] Slots { protected set; get; } = new InventorySlot<TItem, TItemType>[10];

        public Inventory(string inventoryName)
        {
            InventoryName = inventoryName;
            Slots = new InventorySlot<TItem, TItemType>[24];
        }

        public Inventory(string inventoryName, int amountOfSlots)
        {
            InventoryName = inventoryName;
            Slots = new InventorySlot<TItem, TItemType>[amountOfSlots];
        }

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
        public InventorySlot<TItem, TItemType> FindItem(TItem item)
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
        public InventorySlot<TItem, TItemType> SetFirstEmptySlot(TItem item, int amount)
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
        public bool AddItem(TItem item, int amount, InventoryHandler<TItem, TItemType> inventoryHandler)
        {
            InventorySlot<TItem, TItemType> inventorySlot = FindItem(item);
            if (inventoryHandler.Database.Items[item.ID].Stackable && inventorySlot != null)
            {
                inventorySlot.AddAmount(amount);
                return true;
            }

            if (EmptySlotCount <= 0)
                return false;

            if (!inventoryHandler.Database.Items[item.ID].Stackable || inventorySlot == null)
            {
                SetFirstEmptySlot(item, amount);
                return true;
            }

            return false;
        }

        public bool AddItem(InventoryItemObject<TItem, TItemType> itemObject, int amount, InventoryHandler<TItem, TItemType> inventoryHandler)
        {
            return AddItem(itemObject.data, amount, inventoryHandler);
        }

        public void Clear()
        {
            foreach (InventorySlot<TItem, TItemType> slot in Slots)
            {
                slot.UpdateSlot(new TItem(), 0);
            }
        }
    }
}