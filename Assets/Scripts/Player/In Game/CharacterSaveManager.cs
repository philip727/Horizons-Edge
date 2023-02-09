using Philip.Inventory;
using UnityEngine;

public class CharacterSaveManager : MonoBehaviour
{
    [field: SerializeField] public InventoryHandler<Item, ItemObject.ItemGroup> InventoryHandler { private set; get; } = new InventoryHandler<Item, ItemObject.ItemGroup>();
    [field:SerializeField] public PlayerSaveObject PlayerSaveObject { private set; get; }

    private void Start()
    {
        PlayerSaveObject.Load("default");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.J)) 
        {
            PlayerSaveObject.SaveableObject.Inventory.AddItem(InventoryHandler.GetItem(0), 1, InventoryHandler);
        }
        if (Input.GetKey(KeyCode.K))
        {
            PlayerSaveObject.SaveableObject.Inventory.AddItem(InventoryHandler.GetItem(1), 1, InventoryHandler);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerSaveObject.Save("default");
        PlayerSaveObject.Clear();
    }
}
