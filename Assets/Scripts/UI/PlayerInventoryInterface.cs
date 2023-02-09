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
        SlotsOnInterface.Add(obj, slot);
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
        if(slot.Item.ID >= 0)
        {
            if (slot.SlotDisplay == null) return;
            Image slotImage = slot.SlotDisplay.transform.GetChild(2).GetComponent<Image>();
            TextMeshProUGUI amountText = slot.SlotDisplay.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

            slotImage.sprite = _characterSaveManager.InventoryHandler.GetItem(slot.Item.ID).DisplaySprite;
            slotImage.color = new Color(1, 1, 1, 1);
            amountText.text = slot.Amount.FormatLong("0:X");
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

    protected override void OnEnterInterface(GameObject obj)
    {
        InventoryMouseData<Item, ItemObject.ItemGroup>.s_interfaceMouseIsOver = obj.GetComponent<InventoryInterface<Item, ItemObject.ItemGroup>>();
    }

    protected override void OnExitInterface(GameObject obj)
    {
        InventoryMouseData<Item, ItemObject.ItemGroup>.s_interfaceMouseIsOver = null;
    }

    protected override void OnEnter(GameObject obj)
    {
        InventoryMouseData<Item, ItemObject.ItemGroup>.s_slotHoveredOver = obj;
        InventoryMouseData<Item, ItemObject.ItemGroup>.s_interfaceMouseIsOver = this;

        Debug.Log(InventoryMouseData<Item, ItemObject.ItemGroup>.s_slotHoveredOver);
    }

    protected override void OnExit(GameObject obj)
    {
        InventoryMouseData<Item, ItemObject.ItemGroup>.s_slotHoveredOver = null;
        InventoryMouseData<Item, ItemObject.ItemGroup>.s_interfaceMouseIsOver = this;
    }

    protected override void OnDragStart(GameObject obj)
    {
        InventoryMouseData<Item, ItemObject.ItemGroup>.s_TempItemBeingDragged = CreateTempItem(obj);
    }

    protected override void WhilstDrag(GameObject obj)
    {
        if(InventoryMouseData<Item, ItemObject.ItemGroup>.s_TempItemBeingDragged != null)
        {
            InventoryMouseData<Item, ItemObject.ItemGroup>.s_TempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    protected override void OnDragEnd(GameObject obj)
    {
        Destroy(InventoryMouseData<Item, ItemObject.ItemGroup>.s_TempItemBeingDragged);

        if(InventoryMouseData<Item, ItemObject.ItemGroup>.s_interfaceMouseIsOver == null && SlotsOnInterface[obj] != null)
        {
            // do something
        }

        if(InventoryMouseData<Item, ItemObject.ItemGroup>.s_slotHoveredOver)
        {
            var currentInterface = InventoryMouseData<Item, ItemObject.ItemGroup>.s_interfaceMouseIsOver;
            var currentSlotHovered = InventoryMouseData<Item, ItemObject.ItemGroup>.s_slotHoveredOver;
            InventorySlot <Item, ItemObject.ItemGroup> mouseHoverSlotData = currentInterface.SlotsOnInterface[currentSlotHovered];
            Debug.Log("swapping");
            mouseHoverSlotData.Container.SwapItems(SlotsOnInterface[obj], mouseHoverSlotData, _characterSaveManager.InventoryHandler);
        }
    }

    protected override GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (SlotsOnInterface[obj].Item.ID >= 0)
        {
            tempItem = new GameObject();
            RectTransform tempItemRect = tempItem.AddComponent<RectTransform>();
            tempItemRect.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(_temporaryItemParent);

            Image tempItemImage = tempItem.AddComponent<Image>();
            tempItemImage.sprite = _characterSaveManager.InventoryHandler.Database.Items[SlotsOnInterface[obj].Item.ID].DisplaySprite;
            tempItemImage.raycastTarget = false;
        }

        return tempItem;
    }
}
