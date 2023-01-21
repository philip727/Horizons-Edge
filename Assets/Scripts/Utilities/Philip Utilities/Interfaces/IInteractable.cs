using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    [SerializeField] string NameToShow { get; }
    [SerializeField] float InteractRange { get; }
    [SerializeField] bool CanInteract { get; }

    void OnInteract();

}
