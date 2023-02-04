using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Philip.Inventory
{
    public abstract class InventoryItemDatabase<T> : ScriptableObject where T : InventoryItem, new()
    {
        [field: SerializeField] public InventoryItemObject<T>[] Items { private set; get; } = new InventoryItemObject<T>[0];

        [ContextMenu("Update ID's")]
        public virtual void UpdateIDs()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].data.SetID(i);
            }
        }
    }
}
