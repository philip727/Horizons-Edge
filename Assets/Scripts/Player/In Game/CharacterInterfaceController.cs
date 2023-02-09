using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInterfaceController : MonoBehaviour
{
    [SerializeField] private CharacterInputController _characterInputController;

    [SerializeField] private GameObject _inventoryUI;
    public bool IsInventoryOpen 
    { 
        get
        {
            return _inventoryUI.activeSelf;
        } 
    }

    private InputAction _inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(_characterInputController.WaitForInputController(EnableInputs));
    }

    private void OnDisable()
    {
        _inventory.Disable();
    }

    private void EnableInputs()
    {
        _inventory = _characterInputController.CharacterInputActions.Player.Inventory;
        _inventory.Enable();

        _inventory.performed += OnInventoryPressed;
    }

    private void OnInventoryPressed(InputAction.CallbackContext obj)
    {
        _inventoryUI.SetActive(!_inventoryUI.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
