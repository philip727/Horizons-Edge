using System;
using UnityEngine;

namespace Philip.Inventory
{
    public class InventoryItemObject<TItem, TItemType> : ScriptableObject where TItem : InventoryItem, new() where TItemType : Enum
    {
        [field:SerializeField] public Sprite DisplaySprite { private set; get; }
        [field: SerializeField] public bool Stackable { private set; get; }
        [field: SerializeField, TextArea(5, 7)] public string Description { private set; get; }
        [field: SerializeField] public TItemType ItemType { private set; get; }
        [field: SerializeField] public TItem Data { private set; get; } = new TItem();
        public virtual TItem CreateItem()
        {
            return new TItem();
        }
    }
}

