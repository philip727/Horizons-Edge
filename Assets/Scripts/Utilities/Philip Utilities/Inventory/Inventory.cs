using UnityEngine;
using System;
namespace Philip.Inventory
{
    [System.Serializable]
    public class Inventory<TItem, TItemType> where TItem : InventoryItem, new() where TItemType : Enum
    {
        public string InventoryName { private set; get; }
        [field: SerializeField] public InventorySlot<TItem, TItemType>[] Slots { protected set; get; } = new InventorySlot<TItem, TItemType>[10];

        // Creates the inventory with default amount
        public Inventory(string inventoryName)
        {
            InventoryName = inventoryName;
            Slots = new InventorySlot<TItem, TItemType>[24];
            ConstructSlots();
        }

        // Creates the inventory with a specific amount of slots
        public Inventory(string inventoryName, int amountOfSlots)
        {
            InventoryName = inventoryName;
            Slots = new InventorySlot<TItem, TItemType>[amountOfSlots];
            ConstructSlots();
        }

        // Makes sure the slots have the right containers
        private void ConstructSlots()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i] = new InventorySlot<TItem, TItemType>(this);
            }
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

        public void SwapItems(InventorySlot<TItem, TItemType> slot1, InventorySlot<TItem, TItemType> slot2, InventoryHandler<TItem, TItemType> inventoryHandler)
        {
            InventoryItemObject<TItem, TItemType> slot1ItemObject = inventoryHandler.GetItem(slot1.Item.ID);
            InventoryItemObject<TItem, TItemType> slot2ItemObject = inventoryHandler.GetItem(slot2.Item.ID);

            if (slot2.CanPlaceInSlot(slot1ItemObject, slot1.Item) && slot1.CanPlaceInSlot(slot2ItemObject, slot2.Item))
            {
                if (slot1.Item.ID >= 0 && slot2.Item.ID >= 0)
                {
                    if (slot2ItemObject.Stackable && slot1ItemObject.Stackable && (slot1.Item.ID == slot2.Item.ID))
                    {
                        if (slot1 == slot2)
                        {
                            return;
                        }

                        slot2.AddAmount(slot1.Amount);
                        slot1.RemoveItem();
                    }
                }

                InventorySlot<TItem, TItemType> temp = new InventorySlot<TItem, TItemType>(slot2.Item, slot2.Amount, this);
                slot2.UpdateSlot(slot1.Item, slot1.Amount);
                slot1.UpdateSlot(temp.Item, temp.Amount);
            }

        }

        // Adds an item
        public bool AddItem(TItem item, int amount, InventoryHandler<TItem, TItemType> inventoryHandler)
        {
            InventorySlot<TItem, TItemType> inventorySlot = FindItem(item);
            if (inventoryHandler.GetItem(item.ID).Stackable && inventorySlot != null)
            {
                inventorySlot.AddAmount(amount);
                return true;
            }

            if (EmptySlotCount <= 0)
                return false;

            if (!inventoryHandler.GetItem(item.ID).Stackable || inventorySlot == null)
            {
                SetFirstEmptySlot(item, amount);
                return true;
            }

            return false;
        }

        public bool AddItem(InventoryItemObject<TItem, TItemType> itemObject, int amount, InventoryHandler<TItem, TItemType> inventoryHandler)
        {
            return AddItem(itemObject.Data, amount, inventoryHandler);
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