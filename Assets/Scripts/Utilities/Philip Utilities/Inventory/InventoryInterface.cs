using Philip.Utilities.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Philip.Inventory
{
    public abstract class InventoryInterface<TItem, TItemGroup> : MonoBehaviour where TItem : InventoryItem, new() where TItemGroup : System.Enum
    {
        public Dictionary<GameObject, InventorySlot<TItem, TItemGroup>> SlotsOnInterface { protected set; get; }
            = new Dictionary<GameObject, InventorySlot<TItem, TItemGroup>>();

        [SerializeField, Header("Interface")] protected Transform _interfaceParent;

        [SerializeField, Header("Save")] protected CharacterSaveManager _characterSaveManager;

        [SerializeField, Header("Slots")] protected GameObject _slotPrefab;
        [SerializeField] protected Transform _slotsParent;
        [SerializeField] protected Transform _temporaryItemParent;

        protected virtual void Awake()
        {
            PEvent.AddEvent(_slotsParent.gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(_interfaceParent.gameObject); });
            PEvent.AddEvent(_slotsParent.gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(_interfaceParent.gameObject); });
        }

        // Creates a slot for a single inventory slot
        protected abstract void CreateSlot(InventorySlot<TItem, TItemGroup> slot, GameObject obj);

        // Creates all the slots for the required inventories
        protected abstract void CreateAllSlots();

        // Creates all the slots that need delegates
        protected abstract void CreateAllSlotDelegates();

        protected virtual void CreateSlotDelegates(InventorySlot<TItem, TItemGroup> slot)
        {
            slot.OnAfterUpdate += OnSlotUpdate;
        }

        protected virtual void OnSlotUpdate(InventorySlot<TItem, TItemGroup> slot)
        {
            UpdateSlotAppearance(slot);
        }

        protected abstract void UpdateSlotAppearance(InventorySlot<TItem, TItemGroup> slot);
        
        protected virtual void OnSelect(GameObject obj)
        {
            throw new System.NotImplementedException("Not implemented OnSelect(GameObject)");
        }

        protected abstract void OnEnterInterface(GameObject obj);
        protected abstract void OnExitInterface(GameObject obj);
        protected abstract void OnEnter(GameObject obj);
        protected abstract void OnExit(GameObject obj);
        protected abstract void OnDragStart(GameObject obj);
        protected abstract GameObject CreateTempItem(GameObject obj);
        protected abstract void OnDragEnd(GameObject obj);
        protected abstract void WhilstDrag(GameObject obj);

    }
}

