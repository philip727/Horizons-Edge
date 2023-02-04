using Philip.Building;
using Philip.Utilities.Maths;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableResource : StrictChunkBehaviour, IInteractable, IDamageable
{
    public delegate void OnItemInteractable(GameObject gameObject, bool canInteract);
    [field: SerializeField] public string NameToShow { private set; get; }

    [field: SerializeField] public float InteractRange { private set; get; }

    [field: SerializeField] public bool CanInteract { private set; get; }

    [field: SerializeField] public int MaxHealth { private set; get; }

    [field: SerializeField] public int Health { private set; get; }

    [field: SerializeField] public GameObject InteractObject { private set; get; }

    private GameObject _player;

    private CharacterInteractionManager _characterInteraction;

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
        }

        if(_characterInteraction == null && _player != null)
        {
            _characterInteraction = _player.GetComponentInChildren<CharacterInteractionManager>();
        }
    }

    private void CheckInteraction()
    {
        if (!gameObject.activeSelf) return;

        if (PVector.GetDistanceBetween(gameObject, _player, InteractRange, true) && !CanInteract)
        {
            CanInteract = true;
            _characterInteraction.interactables.Add(this);
        }
        else if (CanInteract)
        {
            CanInteract = false;
            _characterInteraction.interactables.Remove(this);
        }
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
