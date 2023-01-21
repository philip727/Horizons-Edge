using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Philip.Inventory
{
    public class InventoryItemObject<T> : ScriptableObject where T : InventoryItem, new()
    {
        [field:SerializeField] public Sprite DisplaySprite { private set; get; }
        [field: SerializeField] public bool Stackable { private set; get; }
        [field: SerializeField, TextArea(5, 7)] public string Description { private set; get; }

        public T data = new T();
        public virtual T CreateItem()
        {
            return new T();
        }
    }
}

