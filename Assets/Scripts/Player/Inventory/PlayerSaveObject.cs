using Philip.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Save Object", menuName = "Philip/Inventory/Save Object")]
public class PlayerSaveObject : SaveObject<PlayerData>
{
    public void Clear()
    {
        SaveableObject.Inventory.Clear();
    }
}
