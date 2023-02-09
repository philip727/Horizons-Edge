using Philip.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryGroundItem<TItem, TItemType> : MonoBehaviour where TItem : InventoryItem, new() where TItemType : System.Enum, IInteractable, ISerializationCallbackReceiver
{
    [field: SerializeField] public InventoryItemObject<TItem, TItemType> ItemObject { private set; get; }
    [field: SerializeField] public long Amount { private set; get; }

    [SerializeField] protected SpriteRenderer _spriteRenderer;

    public void Init(InventoryItemObject<TItem, TItemType> itemObject, long amount)
    {
        ItemObject = itemObject;
        Amount = amount;

        UpdateSpriteRenderer();
    }

    protected void UpdateSpriteRenderer()
    {
        _spriteRenderer.sprite = ItemObject.DisplaySprite;
    }
}
