using Philip.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryInterface<TItem, TItemGroup> : MonoBehaviour where TItem : InventoryItem, new() where TItemGroup : System.Enum
{
    protected Dictionary<GameObject, InventorySlot<TItem, TItemGroup>> _slotsOnInterface 
        = new Dictionary<GameObject, InventorySlot<TItem, TItemGroup>>();

    [SerializeField] protected GameObject _slotPrefab;
    [SerializeField] protected Transform _slotsParent;
    [SerializeField] protected Transform _temporaryItemParent;

    protected virtual void Awake()
    {

    }

    protected virtual void OnInventoryLoaded(PlayerData saveableObject)
    {
        
    }

    // Creates all the slots for the required inventories
    protected abstract void CreateAllSlots();

    // Creates a slot for a single inventory slot
    public abstract void CreateSlot(InventorySlot<TItem, TItemGroup> slot, GameObject obj);

}
