using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Philip.Inventory
{
    public abstract class InventoryItemDatabase<T, TItemType> : ScriptableObject where T : InventoryItem, new() where TItemType : System.Enum
    {
        [field: SerializeField] public InventoryItemObject<T, TItemType>[] Items { private set; get; } = new InventoryItemObject<T, TItemType>[0];

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
