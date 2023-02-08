using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Philip.Inventory
{
    public abstract class InventoryInterface<TItem, TItemGroup> : MonoBehaviour where TItem : InventoryItem, new() where TItemGroup : System.Enum
    {
        protected Dictionary<GameObject, InventorySlot<TItem, TItemGroup>> _slotsOnInterface 
            = new Dictionary<GameObject, InventorySlot<TItem, TItemGroup>>();

        [SerializeField] protected GameObject _slotPrefab;
        [SerializeField] protected Transform _slotsParent;
        [SerializeField] protected Transform _temporaryItemParent;


        [SerializeField] protected CharacterSaveManager _characterSaveManager;

        protected virtual void Awake()
        {

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

        protected GameObject CreateTempItem(GameObject obj)
        {
            GameObject tempItem = null;
            if (_slotsOnInterface[obj].Item.ID >= 0)
            {
                tempItem = new GameObject();
                RectTransform tempItemRect = tempItem.AddComponent<RectTransform>();
                tempItemRect.sizeDelta = new Vector2(50, 50);
                tempItem.transform.SetParent(_temporaryItemParent);

                Image tempItemImage = tempItem.AddComponent<Image>();
                tempItemImage.sprite = _characterSaveManager.InventoryHandler.Database.Items[_slotsOnInterface[obj].Item.ID].DisplaySprite;
                tempItemImage.raycastTarget = false;
            }

            return tempItem;
        }

        protected abstract void OnDragEnd(GameObject obj);
        protected abstract void WhilstDrag(GameObject obj);

    }
}

