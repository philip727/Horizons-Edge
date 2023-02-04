using Philip.Building;
using Philip.Utilities.Maths;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableResource : ChunkRunObject, IInteractable, IDamageable
{
    [field: SerializeField] public string NameToShow { private set; get; }

    [field: SerializeField] public float InteractRange { private set; get; }

    [field: SerializeField] public bool CanInteract { private set; get; }

    [field: SerializeField] public int MaxHealth { private set; get; }

    [field: SerializeField] public int Health { private set; get; }

    private GameObject _player;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!_objectInViewersChunk) return;
        GrabPlayer();
        CheckInteraction();
    }

    private void GrabPlayer()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("Player is null");
        }
    }

    private void CheckInteraction()
    {
        if (!gameObject.activeSelf) return;

        if (PVector.GetDistanceBetween(gameObject, _player, InteractRange, true))
        {
            CanInteract = true;
            Debug.Log("In range");
            return;
        }

        CanInteract = false;
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
