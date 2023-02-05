using Philip.Building;
using Philip.Grid;
using Philip.Utilities.Extras;
using Philip.WorldGeneration;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteractionManager : MonoBehaviour
{
    public readonly List<IInteractable> interactables = new List<IInteractable>();
    [SerializeField, Header("Input")] private CharacterInputController _characterInputController;

    [SerializeField, Header("Highlight Gameobjects")] private Transform _topLeftHighlight;
    [SerializeField] private Transform _topRightHighlight;
    [SerializeField] private Transform _bottomLeftHighlight;
    [SerializeField] private Transform _bottomRightHighlight;

    private Vector2Int _lastCoordinates = default;
    private IInteractable _lastBestInteractable = null;
    private WorldNode _currentMouseNode = null;

    private InputAction _interaction;

    private void OnEnable()
    {
        StartCoroutine(_characterInputController.WaitForInputController(EnableInputs));
    }

    private void OnDisable()
    {
        _interaction.Disable();
    }

    private void EnableInputs()
    {
        _interaction = _characterInputController.CharacterInputActions.Player.Interaction;
        _interaction.Enable();

        _interaction.performed += OnInteractPressed;
    }

    public void Update()
    {
        UpdateMouseNode();
        InteractionHighlighter();
    }

    private void OnInteractPressed(InputAction.CallbackContext obj)
    {
        if (CheckCoords(_currentMouseNode.Coordinates, _lastCoordinates, _lastBestInteractable))
        {
            _lastBestInteractable.OnInteract(obj.control.displayName, this);
        }
    }

    private bool CheckCoords(Vector2Int givenCoordinates, Vector2Int coordinatesToCheck, IInteractable interactable)
    {
        if (interactable == null || interactable.InteractObject == null) return false;
        StructureObject structureObject = interactable.InteractObject.GetComponentInChildren<StructureObject>();
        if (structureObject)
        {
            foreach (Vector2Int coords in structureObject.StructureObjectSettings.CoordinatesItTakesUp)
            {
                if(givenCoordinates == coordinatesToCheck + coords)
                {
                    return true;
                }
            }
        }
        else
        {
            if (givenCoordinates == coordinatesToCheck)
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateMouseNode()
    {
        Vector3 mousePosition = PMouse.GetScreenMouseWorldPosition(Camera.main);
        Grid<WorldNode> grid = WorldGenerationHandler.s_worldData.WorldGrid;
        if (!grid.IsValidCoordinate(grid.GetCoordinate(mousePosition))) return;
        _currentMouseNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(mousePosition);
    }

    private void InteractionHighlighter()
    {
        if (_currentMouseNode == null) return;
        if (CheckCoords(_currentMouseNode.Coordinates, _lastCoordinates, _lastBestInteractable)) 
        {
            DetermineStructure(_lastBestInteractable);
            return;
        }

        GetBestInteractable(_currentMouseNode);
        if (_lastBestInteractable == null)  
        {
            _lastCoordinates = default;
            _topLeftHighlight.gameObject.SetActive(false);
            _topRightHighlight.gameObject.SetActive(false);
            _bottomLeftHighlight.gameObject.SetActive(false);
            _bottomRightHighlight.gameObject.SetActive(false);
            return;
        }

        //DetermineStructure(_lastBestInteractable);
    }


    private void DetermineStructure(IInteractable interactable)
    {
        if (interactable == null) return;
        StructureObject structureObject = interactable.InteractObject.GetComponentInChildren<StructureObject>();

        GetObjectCorners(structureObject);
    }


    private void GetObjectCorners(StructureObject structureObject)
    {
        int lowestX = int.MaxValue;
        int highestX = int.MinValue;
        int lowestY = int.MaxValue;
        int highestY = int.MinValue;

        foreach (Vector2Int coordinates in structureObject.StructureObjectSettings.CoordinatesItTakesUp)
        {
            if (coordinates.x < lowestX)
            {
                lowestX = coordinates.x;
            }

            if (coordinates.x > highestX)
            {
                highestX = coordinates.x;
            }

            if (coordinates.y < lowestY)
            {
                lowestY = coordinates.y;
            }

            if (coordinates.y > highestY)
            {
                highestY = coordinates.y;
            }
        }

        MoveInteractionHighlighter(lowestX, lowestY, highestX, highestY, structureObject.transform.position);
    }

    private void MoveInteractionHighlighter(int lowestX, int lowestY, int highestX, int highestY, Vector3 originalPosition)
    {
        Grid<WorldNode> grid = WorldGenerationHandler.s_worldData.WorldGrid;
        WorldNode worldNode = grid.GetGridObject(originalPosition);
        Vector2Int topLeftCoordinates = worldNode.Coordinates + new Vector2Int(lowestX, highestY);
        Vector2Int topRightCoordinates = worldNode.Coordinates + new Vector2Int(highestY, highestY);
        Vector2Int bottomLeftCoordinates = worldNode.Coordinates + new Vector2Int(lowestX, lowestY);
        Vector2Int bottomRightCoordinates = worldNode.Coordinates + new Vector2Int(highestX, lowestY);

        _topLeftHighlight.position = Vector3.Lerp(_topLeftHighlight.position, grid.GetWorldPosition(topLeftCoordinates), 0.2f);
        _topRightHighlight.position = Vector3.Lerp(_topRightHighlight.position, grid.GetWorldPosition(topRightCoordinates), 0.2f);
        _bottomLeftHighlight.position = Vector3.Lerp(_bottomLeftHighlight.position, grid.GetWorldPosition(bottomLeftCoordinates), 0.2f);
        _bottomRightHighlight.position = Vector3.Lerp(_bottomRightHighlight.position, grid.GetWorldPosition(bottomRightCoordinates), 0.2f);

        _topLeftHighlight.gameObject.SetActive(true);
        _topRightHighlight.gameObject.SetActive(true);
        _bottomLeftHighlight.gameObject.SetActive(true);
        _bottomRightHighlight.gameObject.SetActive(true);
    }

    private bool CheckInteractable(IInteractable interactable, WorldNode mouseNode, WorldNode objectNode)
    {
        StructureObject structureObject = interactable.InteractObject.GetComponentInChildren<StructureObject>();
        if (structureObject)
        {
            foreach (Vector2Int coordinates in structureObject.StructureObjectSettings.CoordinatesItTakesUp)
            {
                if (mouseNode.Coordinates == (objectNode.Coordinates + coordinates))
                {
                    return true;
                }
            }
        }
        else
        {
            if (mouseNode.Coordinates == objectNode.Coordinates)
            {
                return true;
            }
        }

        return false;
    }

    private void GetBestInteractable(WorldNode mouseNode)
    {
        foreach (IInteractable interactable in interactables)
        {
            if(interactable.InteractObject == null)
            {
                interactables.Remove(interactable);
                break;
            }
            Vector3 objectPosition = interactable.InteractObject.transform.position;
            WorldNode objectNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(objectPosition);
            if(CheckInteractable(interactable, mouseNode, objectNode))
            {
                _lastCoordinates = objectNode.Coordinates;
                _lastBestInteractable = interactable;
                return;
            }
        }

        _lastBestInteractable = null;
    }
}
