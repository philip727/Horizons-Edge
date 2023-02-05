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
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            PlayerSaveObject.SaveableObject.Inventory.AddItem(InventoryHandler.Database.Items[0], 1, InventoryHandler);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerSaveObject.Save("default");
        PlayerSaveObject.Clear();
    }
}
