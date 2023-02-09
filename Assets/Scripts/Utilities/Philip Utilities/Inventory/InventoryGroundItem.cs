using Philip.Inventory;
using UnityEngine;

public abstract class InventoryGroundItem<TItem, TItemType> : MonoBehaviour where TItem : InventoryItem, new() where TItemType : System.Enum
{
    [field: SerializeField] public InventoryItemObject<TItem, TItemType> ItemObject { private set; get; }
    [field: SerializeField] public long Amount { private set; get; }

    [SerializeField] protected SpriteRenderer _spriteRenderer;

    public void Init(InventoryItemObject<TItem, TItemType> itemObject, long amount)
    {
        ItemObject = itemObject;
        Amount = System.Math.Clamp(amount, 1, long.MaxValue);
        UpdateSpriteRenderer();
    }

    protected void UpdateSpriteRenderer()
    {
        _spriteRenderer.sprite = ItemObject.DisplaySprite;
    }

    protected virtual void UpdateGameObject()
    {
        if (ItemObject == null) return;
        gameObject.name = $"groundItem_{ItemObject.Data.Name}";
        UpdateSpriteRenderer();
    }
}
