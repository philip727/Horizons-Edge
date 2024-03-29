using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    [SerializeField] string NameToShow { get; }
    [SerializeField] float InteractRange { get; }
    [SerializeField] bool CanInteract { get; }
    [SerializeField] bool SpatialInteraction { get; }
    [SerializeField] GameObject InteractObject { get; }
    void OnInteract(string interactionDisplayName, object initiator);

}
