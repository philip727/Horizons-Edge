using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementController : MonoBehaviour
{
    [field:SerializeField] public GameObject CharacterMainObject { private set; get; }

    private InputAction _movement;

    private void OnEnable()
    {
        StartCoroutine(CharacterInputController.WaitForInputController(EnableInputs));
    }

    private void FixedUpdate()
    {
        ExecuteMovement();
    }

    private void OnDisable()
    {
        _movement.Disable();
    }

    private void EnableInputs()
    {
        _movement = CharacterInputController.CharacterInputActions.Player.Move;

        _movement.Enable();
    }

    private void ExecuteMovement()
    {
        if (_movement != null)
        {
            Vector2 movementVector = _movement.ReadValue<Vector2>();
        }
    }
}
