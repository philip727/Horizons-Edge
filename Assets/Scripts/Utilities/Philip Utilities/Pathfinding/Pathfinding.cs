using Philip.Grid;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Philip.AI
{
    [System.Serializable]
    public class Pathfinding
    {
        public static Pathfinding Instance { private set; get; }

        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        [SerializeField] private Grid<PathNode> _grid;
        private List<PathNode> _openList;
        private List<PathNode> _closedList;
        public Pathfinding(int width, int height, float cellSize = 1f, Vector2 originPosition = default)
        {
            Instance = this;
            _grid = new Grid<PathNode>(width, height, cellSize, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y), debug: false, originPosition: originPosition);
        }

        // Returns the grid used for the pathfinding
        public Grid<PathNode> GetGrid()
        {
            return _grid;
        }

        public static bool IsPositionOnPathfindingGrid(Vector3 worldPositin)
        {
            PathNode node = Instance._grid.GetGridObject(worldPositin);
            return Instance._grid.IsValidCoordinate(node.X, node.Y);
        }

        public static bool IsPositionOnPathfindingGrid(Vector2Int coords)
        {
            return Instance._grid.IsValidCoordinate(coords.x, coords.y);
        }

        // Checks if a node is walkable
        public bool IsNodeWalkable(Vector2Int coords)
        {
            return _grid.GetGridObject(coords.x, coords.y).IsWalkable;
        }

        // Finds a path from one coordinate to the other
        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = _grid.GetGridObject(startX, startY);
            PathNode endNode = _grid.GetGridObject(endX, endY);

            _openList = new List<PathNode>() { startNode };
            _closedList = new List<PathNode>();

            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    PathNode pathNode = _grid.GetGridObject(x, y);
                    pathNode.SetWalkingCost(int.MaxValue);
                    pathNode.CalculateFinalCost();
                    pathNode.UpdateLastNode(null);
                }
            }

            startNode.SetWalkingCost(0);
            startNode.SetHeuristicCost(CalculateDistance(startNode, endNode));
            startNode.CalculateFinalCost();

            while (_openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(_openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                _openList.Remove(currentNode);
                _closedList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (_closedList.Contains(neighbourNode)) continue;
                    if(!neighbourNode.IsWalkable)
                    {
                        _closedList.Add(neighbourNode);
                        continue;
                    }
                    int tentativeWalkingCost = currentNode.WalkingCost + CalculateDistance(currentNode, neighbourNode);

                    if(tentativeWalkingCost < neighbourNode.WalkingCost)
                    {
                        neighbourNode.UpdateLastNode(currentNode);
                        neighbourNode.SetWalkingCost(tentativeWalkingCost);
                        neighbourNode.SetHeuristicCost(CalculateDistance(neighbourNode, endNode));
                        neighbourNode.CalculateFinalCost();

                        if(!_openList.Contains(neighbourNode))
                        {
                            _openList.Add(neighbourNode);
                        }
                    }

                }
            }

            // Out of nodes on the openlist
            return null;
        }

        // Gets all the nodes around that node
        private List<PathNode> GetNeighbourList(PathNode node)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            if (node.X - 1 >= 0)
            {
                // Left
                neighbourList.Add(GetNode(node.X - 1, node.Y));

                // Left Down
                if (node.Y - 1 >= 0) neighbourList.Add(GetNode(node.X - 1, node.Y - 1));

                // Left Up
                if (node.Y + 1 < _grid.Height) neighbourList.Add(GetNode(node.X - 1, node.Y + 1));
            }
            if (node.X + 1 < _grid.Width)
            {
                // Right
                neighbourList.Add(GetNode(node.X + 1, node.Y));

                // Right Down
                if (node.Y - 1 >= 0) neighbourList.Add(GetNode(node.X + 1, node.Y - 1));

                // Right Up
                if (node.Y + 1 < _grid.Height) neighbourList.Add(GetNode(node.X, node.Y + 1));
            }
            // Down
            if (node.Y - 1 >= 0) neighbourList.Add(GetNode(node.X, node.Y - 1));

            // Up
            if (node.Y + 1 < _grid.Height) neighbourList.Add(GetNode(node.X, node.Y + 1));

            return neighbourList;
        }

        // Gets an exact node
        private PathNode GetNode(int x, int y)
        {
            return _grid.GetGridObject(x, y);
        }

        // Calculates the path
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>
            {
                endNode
            };
            PathNode currentNode = endNode;
            while(currentNode.PreviousNode != null)
            {
                path.Add(currentNode.PreviousNode);
                currentNode = currentNode.PreviousNode;
            }
            path.Reverse();
            return path;
        }

        // Calculates the heuristic cost
        private int CalculateDistance(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        // Gets the lowest fcost node
        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FinalCost < lowestFCostNode.FinalCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }

    }


}