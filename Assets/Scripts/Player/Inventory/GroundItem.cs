using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : InventoryGroundItem<Item, ItemObject.ItemGroup>, IInteractable, ISerializationCallbackReceiver
{
    public string NameToShow => throw new System.NotImplementedException();

    public float InteractRange => throw new System.NotImplementedException();

    public bool CanInteract => throw new System.NotImplementedException();

    public GameObject InteractObject => throw new System.NotImplementedException();

    public void OnAfterDeserialize()
    {
      
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        UpdateGameObject();
#endif
    }

    public void OnInteract(string interactionDisplayName, object initiator)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
