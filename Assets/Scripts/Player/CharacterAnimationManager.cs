using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    [SerializeField, Header("Characters References")] private Animator _characterAnimator;
    [SerializeField, Header("Movement")] private CharacterMovementController _characterMovementController;

    public void Update()
    {
        MovementAnimations();
    }

    public void MovementAnimations()
    {
        _characterAnimator.SetInteger("Speed", _characterMovementController.NormalizedSpeed);
    }
}
