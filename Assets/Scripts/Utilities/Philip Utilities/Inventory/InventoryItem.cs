using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Philip.Inventory
{
    [System.Serializable]
    public abstract class InventoryItem
    {
        [field:SerializeField] public string Name { protected set; get; }

        [field:SerializeField] public int ID { protected set; get; }


        public void SetID(int value)
        {
            ID = value;
        }

        public InventoryItem()
        {
            Name = "";
            ID = -1;
        }
    }
}
