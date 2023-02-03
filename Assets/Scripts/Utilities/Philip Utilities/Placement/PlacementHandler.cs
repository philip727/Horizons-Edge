using Philip.Utilities.Extras;
using Philip.Grid;
using Philip.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Philip.Building 
{ 
    public class PlacementHandler : MonoBehaviourSingleton<PlacementHandler>
    {
        public IBuildable CurrentObject { private set; get; } = null;
        public bool IsBuilding { private set; get; }

        private GameObject _currentObjectPreview = null;
        private Grid<PlacementNode<IBuildable>> _grid;
        private Vector2Int currentCoords;


        public override void Awake()
        {
            base.Awake();

        }

        public void OnEnable()
        {
            //StartCoroutine(WaitForInputHandler());
        }

        //private IEnumerator WaitForInputHandler()
        //{
        //    yield return new WaitUntil(() => InputHandler.Instance != null);
        //    //InputHandler.Instance.PlayerInputActions.Placement.Place.performed += PlaceBuilding;
        //    //InputHandler.Instance.PlayerInputActions.Placement.Place.Enable();

        //    //InputHandler.Instance.PlayerInputActions.Placement.ExitBuilding.performed += CancelBuilding;
        //    //InputHandler.Instance.PlayerInputActions.Placement.ExitBuilding.Enable();
        //}

        public void OnDisable()
        {
            //InputHandler.Instance.PlayerInputActions.Placement.Place.Disable();
            //InputHandler.Instance.PlayerInputActions.Placement.ExitBuilding.Disable();
        }

        private void Update()
        {
            BuildingProcess();
        }

        // The building process
        private void BuildingProcess()
        {
            if (IsBuilding && _currentObjectPreview != null)
            {
                // Gets the grid and the current co-ordinate our mouse is at
                _grid = Placement<IBuildable>.Instance.GetGrid();
                currentCoords = _grid.GetCoordinate(Camera.main.GetScreenMouseWorldPosition());

                UpdatePreviewPosition(_grid, currentCoords);

                UpdatePreviewAppearance(CurrentObject, currentCoords);
            }
        }

        // Checks if its a valid coordinate so it doesnt move off screen / off grid
        private void UpdatePreviewPosition(Grid<PlacementNode<IBuildable>> grid, Vector2Int givenCoords)
        {
            if (!grid.IsValidCoordinate(givenCoords)) return;

            _currentObjectPreview.transform.position = grid.GetWorldPosition(givenCoords) + new Vector3(0.05f, 0.05f, 0f);
        }

        // Changes the color of the preview
        private void UpdatePreviewAppearance(IBuildable buildableObject, Vector2Int givenCoords)
        {
            if (!Placement<IBuildable>.Instance.CanPlaceBuildingAtNode(buildableObject, givenCoords))
            {
                    ChangeAppearance(new Color(1f, 0f, 0f, 0.6f));
                    return;
            }

            ChangeAppearance(new Color(0f, 1f, 0f, 0.6f));
        }

        // Changes colour of preview appearance
        private void ChangeAppearance(Color color)
        {
            SpriteRenderer[] sprites = _currentObjectPreview.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = color;
            }
        }

        public void StartBuilding(GameObject gObj)
        {
            if (IsBuilding) return;

            IBuildable buildableObject = gObj.GetComponentInChildren<IBuildable>();

            if(buildableObject == null)
            {
                Debug.LogError("<color=#754deb>[BUILDING] </color> Unable to find IBuildable on object passed");
            }

            IsBuilding = true;

            // Makes sure there is no preview
            if (_currentObjectPreview != null)
            {
                Destroy(_currentObjectPreview);
            }

            // Sets up the current buildable object
            CurrentObject = buildableObject;
            gObj.name = $"{CurrentObject}_preview";
            _currentObjectPreview = gObj;
        }

        // Exits the building process
        public void ExitBuilding()
        {
            IsBuilding = false;

            // Should be set to null when you finish building
            if (_currentObjectPreview != null)
            {
                Destroy(_currentObjectPreview);
            }
            CurrentObject = null;
        }

        // Places the current building
        private void PlaceBuilding(InputAction.CallbackContext obj)
        {
            if (IsBuilding && Placement<IBuildable>.Instance.CanPlaceBuildingAtNode(CurrentObject, currentCoords) && CurrentObject.HasBuildRequirements())
            {
                Placement<IBuildable>.Instance.PlaceObjectInNode(CurrentObject, currentCoords);

                // Makes the tower look normal
                ChangeAppearance(new Color(1, 1, 1, 1));

                // Updates the tower name before its placed
                _currentObjectPreview.name = $"{CurrentObject}";

                // Exits building process
                _currentObjectPreview = null;
                ExitBuilding();
            }
        }

        // Cancels the building process for the placement handler
        private void CancelBuilding(InputAction.CallbackContext obj)
        {
            if (IsBuilding)
            {
                ExitBuilding();
            }
        }
    }
}

