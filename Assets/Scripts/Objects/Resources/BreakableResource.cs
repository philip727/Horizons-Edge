using Philip.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableResource : StructureObject, IInteractable, IDamageable
{
    [field: SerializeField] public string NameToShow { private set; get; }

    [field: SerializeField] public float InteractRange { private set; get; }

    [field: SerializeField] public bool CanInteract { private set; get; }

    [field: SerializeField] public int MaxHealth { private set; get; }

    [field: SerializeField] public int Health { private set; get; }

    protected override void Start()
    {
        base.Start();
    }

    public void Death()
    {
        //throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        //throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        //throw new System.NotImplementedException();
    }
}
