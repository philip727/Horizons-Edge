using Philip.Grid;
using UnityEngine;

namespace Philip.Building
{
    [System.Serializable]
    public class Placement<TBuildingObject> where TBuildingObject : IBuildable
    {
        [SerializeField] private Grid<PlacementNode<TBuildingObject>> _grid;

        public static Placement<TBuildingObject> Instance { private set; get; }

        public Placement(int width, int height, float cellSize = 1f, Vector2 originPosition = default)
        {
            Instance = this;
            _grid = new Grid<PlacementNode<TBuildingObject>>(width, height, cellSize, (Grid<PlacementNode<TBuildingObject>> g, int x, int y) => new PlacementNode<TBuildingObject>(g, x, y), debug: false, originPosition: originPosition);
        }

        public TBuildingObject GetNeighbourNodeInDirection(Vector2Int coords, PlacementDirection direction)
        {
            return direction switch
            {
                PlacementDirection.Above => _grid.GetGridObject(coords.x, coords.y + 1).ObjectInNode,
                PlacementDirection.Right => _grid.GetGridObject(coords.x + 1, coords.y).ObjectInNode,
                PlacementDirection.Left => _grid.GetGridObject(coords.x - 1, coords.y).ObjectInNode,
                PlacementDirection.Below => _grid.GetGridObject(coords.x, coords.y - 1).ObjectInNode,
                _ => default,
            };
        }

        public bool CanPlaceBuildingAtNode(TBuildingObject buildingObject, Vector2Int givenCoords)
        {
            foreach (Vector2Int coords in buildingObject.StructureObjectSettings.CoordinatesItTakesUp)
            {
                if (!_grid.IsValidCoordinate(givenCoords + coords) || !CanPlaceInNode(givenCoords + coords))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanPlaceBuildingAtNode(StructureObjectSettings structureObjectSettings, Vector2Int givenCoords)
        {
            foreach (Vector2Int coords in structureObjectSettings.CoordinatesItTakesUp)
            {
                if (!_grid.IsValidCoordinate(givenCoords + coords) || !CanPlaceInNode(givenCoords + coords))
                {
                    return false;
                }
            }

            return true;
        }

        public void PlaceObjectInNode(TBuildingObject buildingObject, Vector2Int coords)
        {
            PlacementNode<TBuildingObject> node = _grid.GetGridObject(coords.x, coords.y);
            SetNodesAroundNode(node, buildingObject, false);
            node.SetObjectInNode(buildingObject);
            buildingObject.OnBuilt();
            Debug.Log($"<color=#754deb>[BUILDING]</color> Built {buildingObject.BuiltObject.name} at ({coords.x}, {coords.y})");
        }

        public TBuildingObject GetObjectInNode(Vector2Int coords)
        {
            return _grid.GetGridObject(coords).ObjectInNode;
        }

        private void SetNodesAroundNode(PlacementNode<TBuildingObject> node, TBuildingObject buildingObject, bool value)
        {
            foreach (Vector2Int coords in buildingObject.StructureObjectSettings.CoordinatesItTakesUp)
            {
                if (_grid.IsValidCoordinate(node.Coordinates + coords))
                {
                    _grid.GetGridObject(node.Coordinates + coords).SetIsBuildable(value);
                }
            }
        }

        private void SetNodesArondCoordinates(TBuildingObject buildingObject, Vector2Int givenCoords, bool value)
        {
            foreach (Vector2Int coords in buildingObject.StructureObjectSettings.CoordinatesItTakesUp)
            {
                if (_grid.IsValidCoordinate(givenCoords + coords))
                {
                    _grid.GetGridObject(givenCoords + coords).SetIsBuildable(value);
                }
            }
        }

        // Checks if we can place in that node
        public bool CanPlaceInNode(int x, int y)
        {
            PlacementNode<TBuildingObject> node = _grid.GetGridObject(x, y);
            return node.CanBuildOn;
        }

        // Checks if we can place in that node
        public bool CanPlaceInNode(Vector2Int coords)
        {
            PlacementNode<TBuildingObject> node = _grid.GetGridObject(coords.x, coords.y);
            return node.CanBuildOn;
        }

        // Gets the grid the placement node uses
        public Grid<PlacementNode<TBuildingObject>> GetGrid()
        {
            return _grid;
        }
    }
}
