using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Philip.Inventory;
using Philip.Utilities.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Philip.Utilities.String;
using System;

public class PlayerInventoryInterface : InventoryInterface<Item, ItemObject.ItemGroup>
{
    [SerializeField] private PlayerSaveObject _playerSave;
    [SerializeField] private GameObject[] _equipmentSlots;

    protected override void Awake()
    {
        _playerSave.onLoad += OnSaveLoaded;
    }

    private void OnSaveLoaded(PlayerData saveableObject)
    {
        CreateAllSlots();
        CreateAllSlotDelegates();
        ForceUpdateSlots();
    }

    protected override void CreateAllSlots()
    {
        foreach (InventorySlot<Item, ItemObject.ItemGroup> slot in _playerSave.SaveableObject.Inventory.Slots)
        {   
            GameObject createdSlotPrefab = Instantiate(_slotPrefab, _slotsParent);
            CreateSlot(slot, createdSlotPrefab);
        }
        //for (int i = 0; i < _playerSave.SaveableObject.Equipment.Slots.Length; i++)
        //{
        //    InventorySlot<Item, ItemObject.ItemGroup> slot = _playerSave.SaveableObject.Equipment.Slots[i];
        //    GameObject obj = _equipmentSlots[i];
        //    CreateSlot(slot, obj);
        //}
    }

    protected override void CreateSlot(InventorySlot<Item, ItemObject.ItemGroup> slot, GameObject obj)
    {
        slot.SetParentInterface = this;

        PEvent.AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
        PEvent.AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
        PEvent.AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
        PEvent.AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
        PEvent.AddEvent(obj, EventTriggerType.Drag, delegate { WhilstDrag(obj); });
        slot.SetSlotDisplay = obj;
        _slotsOnInterface.Add(obj, slot);
    }

    protected override void CreateAllSlotDelegates()
    {
        foreach (InventorySlot<Item, ItemObject.ItemGroup> slot in _playerSave.SaveableObject.Inventory.Slots)
        {
            CreateSlotDelegates(slot);
        }
        //foreach (InventorySlot<Item, ItemObject.ItemGroup> slot in _playerSave.SaveableObject.Equipment.Slots)
        //{
        //    CreateSlotDelegates(slot);
        //}
    }

    protected void ForceUpdateSlots()
    {
        foreach (InventorySlot<Item, ItemObject.ItemGroup> slot in _playerSave.SaveableObject.Inventory.Slots)
        {
            UpdateSlotAppearance(slot);
        }
    }

    protected override void UpdateSlotAppearance(InventorySlot<Item, ItemObject.ItemGroup> slot)
    {
        Debug.Log("UpdateSlotAppearance()");
        if(slot.Item.ID >= 0)
        {
            if (slot.SlotDisplay == null) return;
            Image slotImage = slot.SlotDisplay.transform.GetChild(2).GetComponent<Image>();
            TextMeshProUGUI amountText = slot.SlotDisplay.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

            slotImage.sprite = _characterSaveManager.InventoryHandler.GetItem(slot.Item.ID).DisplaySprite;
            slotImage.color = new Color(1, 1, 1, 1);
            amountText.text = slot.Amount.FormatLong();
            Debug.Log("ID >= 0");
        }
        else
        {
            if (slot.SlotDisplay == null) return;
            Image slotImage = slot.SlotDisplay.transform.GetChild(2).GetComponent<Image>();
            TextMeshProUGUI amountText = slot.SlotDisplay.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

            slotImage.color = new Color(1, 1, 1, 0);
            amountText.text = "";
        }
    }

    protected override void OnEnter(GameObject obj)
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnEnterInterface(GameObject obj)
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnExit(GameObject obj)
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnExitInterface(GameObject obj)
    {
        //throw new System.NotImplementedException();
    }

    protected override void WhilstDrag(GameObject obj)
    {
        //throw new System.NotImplementedException();
    }
    protected override void OnDragEnd(GameObject obj)
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnDragStart(GameObject obj)
    {
        //throw new System.NotImplementedException();
    }
}
