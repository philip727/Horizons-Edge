using Philip.Utilities.Extras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementController : MonoBehaviour
{
    [field:SerializeField] public Rigidbody2D CharacterRigidbody { private set; get; }

    // Character Input
    [SerializeField, Header("Input")] private CharacterInputController _characterInputController;
    [SerializeField] private float _speed;
    public int NormalizedSpeed { private set; get; }

    // Scale Manipulation
    [SerializeField, Header("Sprite Modification")] private SpriteRenderer _characterSpriteRenderer;
    private float _currentSide = 1;

    private InputAction _movement;

    private void OnEnable()
    {
        StartCoroutine(_characterInputController.WaitForInputController(EnableInputs));
    }

    private void Update()
    {
        AdjustSpriteScale();
        AdjustSide();
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
        _movement = _characterInputController.CharacterInputActions.Player.Move;

        _movement.Enable();
    }

    // Moves character depending on input
    private void ExecuteMovement()
    {
        if (_movement == null) return;

        Vector2 movementVector = _movement.ReadValue<Vector2>();
        NormalizedSpeed = Mathf.Clamp(Mathf.CeilToInt(movementVector.magnitude), 0, 1);

        // Only updates if necessary
        if (movementVector == default) return;

        // Movement based side
        //int temporarySide = Mathf.RoundToInt(movementVector.x);

        // Makes sure the side is never 0, as this would mess up the scaling.
        //_currentSide = temporarySide != 0 ? temporarySide : _currentSide;

        Vector3 moveByVector = CharacterRigidbody.transform.position + (_speed * Time.fixedDeltaTime * new Vector3(movementVector.x, movementVector.y, 0f));
        CharacterRigidbody.MovePosition(moveByVector);
    }

    // Adjusts facing side depending on the mouse
    private void AdjustSide()
    {
        Vector3 mousePosition = PMouse.GetScreenMouseWorldPosition(Camera.main);
        Vector3 characterPosition = CharacterRigidbody.transform.position;
        Vector3 distance = mousePosition - characterPosition;
        int temporarySide = Mathf.Clamp(distance.x < 0f ? Mathf.FloorToInt(distance.x) : Mathf.CeilToInt(distance.x), -1, 1);

        // Only updates when necessary
        if (temporarySide == _currentSide) return;

        // Makes sure the side is never 0, considering the scaling would be very messed up
        _currentSide = temporarySide != 0 ? temporarySide : _currentSide;
    }

    // Makes sure the character is facing the right way
    private void AdjustSpriteScale()
    {
        Vector3 lastScale = _characterSpriteRenderer.transform.localScale;
        //Vector3 lastPosition = _characterSpriteRenderer.transform.localPosition;

        // Only updates when necessary
        if (lastScale.x == _currentSide) return;

        _characterSpriteRenderer.transform.localScale = new Vector3(_currentSide, lastScale.y, lastScale.z);

        // If the sprite pivot point is ever bottom left
        //_characterSpriteRenderer.transform.localPosition = new Vector3((_currentSide * -1) / 2, lastPosition.y, lastPosition.z);
    }
}
