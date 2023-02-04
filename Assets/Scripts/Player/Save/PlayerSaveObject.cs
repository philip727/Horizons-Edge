using Philip.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Save Object", menuName = "Philip/Inventory/Save Object")]
public class PlayerSaveObject : ScriptableObject
{
    [field: SerializeField] public Inventory<Item, ItemObject.ItemGroup> Inventory { private set; get; } = new Inventory<Item, ItemObject.ItemGroup>();
}
